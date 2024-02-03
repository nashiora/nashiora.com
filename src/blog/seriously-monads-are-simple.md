# Seriously, Monads are Simple

It's not a stretch to say that most programmers who have heard of monads don't
actually know what they are. Hell, programmers who've heard of monads are
probably the vast minority of programmers in the first place. Most media I've
found discussing monads have not been approachable to those not already in the
know, usually using academic language or terms unfamiliar to your average
programmer. Even those attempting to avoid assumed prior knowledge on the part
of the reader often struggle to truly be accessible or approachable to your
average programmer curious what the functional crowd is always talking about.

I can only make an attempt to cater to that audience. I can't claim this will be
a perfect introduction or explanation, but I can certainly join the ranks of
those who have tried. So no matter your language or paradigm of choice, I'd like
to introduce you to monads from the perspective of someone who still doesn't
understand all the functional jargon.

## Setting the Stage

I have an application called DOGASS. The purpose of DOGASS 








Let's say I need to read some structure textual data from a hot new format
called DOGASS. I need to write a parser for DOGASS because my language of choice
doesn't have one yet. This shouldn't be a problem because the designers of
DOGASS kept it very simple, only having two value types: strings and key-value
tables of strings.

DOGASS looks something like this:

```
DOG
    "foo": "bar",
    "child": DOG
        "name": "Jacob",
        "seating-preference": "ball"
    ASS
ASS
```

Where the word `DOG` is used to open a key-value table and `ASS` is used to
close it. String values look like any other sane language, delimited by double
quotes.

So to parse this, we should only need two functions: one for string values and
one for tables.

```c#
class DogassParser {
    DogassValue ParseValue();
    DogassTable ParseTable();
}
```

Once I implement these two functions (and the parser behind them, don't worry
about that) then I have a functional DOGASS parser!

## Complications

But what if there's an error in the DOGASS data? A table missing its closing
`ASS`, a value that isn't quoted, or, Dog forbid, a trailing comma! These two
functions don't have a way to tell us if an error occurred.

We could print them to the standard error stream, or to our own custom logging
implementation, but that doesn't force the programmer to consider the error. In
fact, doing so doesn't even *let* the programmer consider the error in the first
place.

Parse errors could be stored in the parser itself, allowing a programmer to
query it for all of the errors occurred during parsing. This lets the programmer
do something about the errors, but only if they think to check what the parser
has to say.

Okay, so maybe we store the error data in the return value itself. The
programmer still has to check, but the error is easily accessible through the
return value and can't be confused for the result of previous parse attempts.
This is the most promissing idea so far, so let's see what it might look like.

```c#
class DogassValue {
    string Value;
    string ErrorMessage;
}

class DogassTable {
    Dictionary<string, DogassValue> KeyValuePairs;
    string ErrorMessage;
}
```

Now both of the results from our parser can store their error information. We
still have to remember to check, though, and the errors can be hard to find when
nested deeply in a table. If we parse something with 3 nested tables and only
the innermost has an error, we have to traverse the whole table structure to get
there and find the error. Maybe we can solve that by having the parser bubble
up errors as it encounters them, but what if there are multiple errors? The
error messages now need to be stored in an array or list instead.

```c#
class DogassValue {
    string Value;
    string ErrorMessage;
}

class DogassTable {
    Dictionary<string, DogassValue> KeyValuePairs;
    List<string> ErrorMessages;
}
```

Now that we can bubble error messages up to the parent table's list of errors,
we'd only have to check there for all of the errors in the program. There's a
downside, now that the more values and tables are parsed, the more redundant
data is stored in our program as all of the parse errors are copied up. We can
move them instead of copying them, leaving all child errors empty again, but
that's extra work for the parser to do.

Even after all the clean up and consolidation of errors to the outermost value,
all of the child values still have empty error data associated with them that's
now completely useless. Sure, we can live with this, but why don't we instead
move the error information into a class that wraps the result of the parse?

```cs
class DogassParseResult<TValue> {
    TValue Value;
    List<string> ErrorMessages;
}

class DogassValue {
    string Value;
}

class DogassTable {
    Dictionary<string, DogassValue> KeyValuePairs;
}

class DogassParser {
    DogassParseResult<DogassValue> ParseValue();
    DogassParseResult<DogassTable> ParseTable();
}
```

Now our parsed values know nothing of the errors generated. We can't possibly
forget to move the errors or mistakenly check empty ones. The parser is still
responsible for moving errors between different results, but the actual values
aren't affected by it anymore.

I'd consider these steps to be routine and expected for many different kinds of
problems, but it may not be obvious to everyone why this separation is useful. I
believe a lot of this understanding comes from personal experience, so if you
haven't written a lot of software for larger projects then thinking about
periodically transforming your data like this might be hard.

## Parse Result: How To Use It

Hopefully the utility of a dedicated parse result type becomes aparent once we
look at how it might be used.

```c#

class DogassParser {
    // Get the current "token", basically word, 
    DogassToken CurrentToken { get; }

    DogassParseResult<DogassValue> ParseValue() {

    }

    DogassParseResult<DogassTable> ParseTable();
}
```
