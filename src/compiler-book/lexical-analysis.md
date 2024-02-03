<div class="number1">$section-roman$</div>

# Lexical Analysis

The first step in any compiler is understanding source code. We write programs
in plain text, which doesn't mean anything to a computer, so some processing
needs to be done to make sense of it. This processing is called parsing: making
sense of the various pieces of text, transforming words and symbols into
constructs with meaning.

If you're not familiar with how compilers work, or the concept of parsing in
general, I encourage you to seriously sit down and think about how you would
solve this problem.

How do you start to assign meaning to various pieces of the
source code? What kind of structure can you create to represent a program?
Programming languages have strict rules that source code must follow to be
considered valid. Think of your favorite programming language and what you
know about its rules, its grammar. How would you represent that?

If you thought of regular expressions, then you're on to something. Regular
expressions allow you to describe the shape of text and extract meaning from it,
and once you're familiar with them, they become incredibly powerful.

There are a couple of problems with regular expressions, though, one of which is
in their name: regular expressions are useful for parsing *regular languages*,
but the vast majority programming languages simply are not regular. The second
issue with regular expressions is their performance: regular expression
evaluators tend to take a significant amount of time to run.

Regular expressions have their place, but inside a compiler is not one of them.

Another option you'll come across when researching compilers is a parser
generator. These automate the process 

### Lexer & Parser Generators

You may be familiar with tools such as `lex` and `yacc`. Tools like these are
usually called parser generators (or "compiler compilers", if you want to be fun
about it). Similar to regular expressions, you provide a parser generator with a
description of your language and ask it to help you build your first
representation of the source code. Parser generators have a leg up over regular
expressions since they often allow you to inject your own code into each step
along the way.

Despite the improvement over regular expressions, we won't be using parser
generators either. Much like regular expressions it can be hard to include
context sensitive language constructs, though the allowance of your own code in
the process means this isn't impossible. The bigger issue with parser generators
is the infamously bad error messages. A parser generator understands what your
language looks like, but it ultimately has no idea what it actually means.
Readable and understandable error messages are therefore hard for them to
provide, and actually useful errors are essentially impossible.

Since we won't be using parser generators, either, I won't linger on the topic
for as long as I did regular expressions. I still reccommend those interested to
look into parser generators and get a taste for them yourself. They aren't
useless, but they aren't a great fit for the kind of language we're going to be
building in this book.

The final nail in the coffin for both regular expressions and parser generators
is that it's just not the kind of thing I want to teach in this book. I like to
encourage people to learn how things really work, so we're going to learn how
to solve this problem on our own, from scratch.

The following chapters will explain how to build your own lexer and parser
completely from scratch.

### What is a Lexer?

A lexer (or "scanner" or "tokenizer", depending on who you ask) is responsible
for the very first step in understanding source code. Its job is to transform
the source text into a rough equivalent of words, called "tokens". A token is
the smallest form of meaning in your language, often represented as a kind plus
some optional unique data. Let's look at some source code to better understand
what a token really is.

```lua
foo = 34
```

This source code contains three tokens. Before reading ahead, see if you can
figure out what those three tokens are and which ones need extra data associated
with them.

The first token, `foo`, is an "identifier", a word in the language used to refer to
values. Identifier tokens will also store the name of the identifier as their
optional value, so we know what names to look up or associated data to.

The second token is the equal sign and requires no extra data to store, so long
as its kind is unique. If we call this token `EQUAL` then there's no question
what it is, but if we instead had a generic `OPERATOR` token, then the text of
the operator would need to be stored along with it to differentiate it from
other operators like `+` or `>`.

The third and final token is the number `34`, which is a number. We can even
specify that this is an integer since there is no fractional component to it.
Numeric tokens will store either the textual representation of the number, to be
processed at a later time, or the value of the number itself if you bother to
keep track of it at this stage.

This doesn't give us any actual structure just yet. What it does is give us is a
significantly more usable way to think about source text. Now we aren't thinking
in terms of characters or arbitrary strings of text, but well defined words that
have meaning.

### What is a Parser?

<aside class="down3">

You'll most often hear the output of a parser referred to as an "Abstract Syntax
Tree", or AST for short. The inclusion of "abstract" refers to the fact that the
resulting tree may not have all of the information about the source text.

In practice I find the term often misused, so for the rest of the book I will
simply refer to it as a syntax tree, no additional qualifiers. When doing your
own research or discussion, just be aware of the term. You'll be perfectly
understood using either.

</aside>

To make sense of these tokens, we need a parser. A parser consumes the tokens
our lexer generates and starts forming even more meaningful chunks of code, most
often structured as a tree. This tree structure is called a "syntax tree", and
it's constructed out of "syntax nodes". Let's look at why the parser would
produce a tree.

An easy to visualize example of the tree-like nature of most programming languages
is conditional logic like an `if` statement:

```lua
if foo then
    return "foo!";
else
    return "not foo...";
end
```

The branches of the `if` statement become the branches of our tree structure.

```
IF
├─IDENTIFIER foo
├─RETURN "foo!"
└─RETURN "not foo..."
```

The root of our tree is the `IF` node, which has three branches. This might be
strange since there's only two branches in the source code, one if the condition
is true and one if it's false, but we also have to consider that condition. The
condition is a child of the `IF` node just like the two branches are. Our
representation of an `if` statement therefore requires three children: one for
the condition, one for the "success" body and one final child for the "failure"
body.

As an exercise, see if you can construct a tree for the following expression:

```lua
1 + 2 * 3 + 4
```

Yes, even arithmetic expressions become trees! Don't worry if you don't have an
answer yet, we'll go over exactly how to turn this into a tree and why in later
chapters on parsing.

<hr />

The next chapter will go into more detail about how a lexer works and get you
started writing one for Hibiku.
