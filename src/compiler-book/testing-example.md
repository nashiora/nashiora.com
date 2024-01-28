<div class="number1">1</div>

# Introduction

I've been fascinated by programming languages for as long as I can recall knowing what they were, and especially so after realizing they were more than just magic that already existed. As soon as I learned someone *made* a programming language I dove head first into trying to make my own. With zero knowledge of how compilers or interpreters worked, I booted up Roblox Studio and tried to create a programming language in Lua. I didn't know what `void` meant, but it looked cool, so that was my function declaration keyword.

Yeah, I didn't get very far.

That brief tangent is to say I remember what it was like to be desperate to make a programming language without any know-how to get the job done. I've come a long way since then and much of the process is second nature to me now, but I've never stopped looking for accessible learning materials. Admitedly, compilers as a whole are not the most accessible topic, especially the closer you get to generating actual executable code, but that doesn't mean they're impossible. I'm choosing to believe that not enough people have truly decided to write easily accessible and beginner friendly articles or books about compilers rather than accept that it's impossible.

<aside class="up2">
This is a test aside to see how I can get them to render with little to no additional work.
</aside>

That's the goal of this series of posts (if I stick with it, maybe I'll call it a "book"). I want to share all I've learned about compilers, and interpreters, and do my best to make the experience both approachable to beginners *and* a good reference material for those already more experienced.

```c
#include <stdio.h>
int main() {
    fprintf(stderr, "Hello, Z2C!\n");
    return 0;
}
```

<div class="number2">1.1</div>

## Why learn about compilers?

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla ut rhoncus ex. Praesent vel tempus dolor. Donec id consequat nulla. Aliquam mi metus, pharetra non posuere sed, sollicitudin vitae leo. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent nec tortor quis magna ullamcorper condimentum. Pellentesque tincidunt massa at condimentum posuere. Mauris a tempus risus, nec dictum leo. In feugiat felis sit amet leo laoreet, eget rutrum libero mollis. Maecenas in sem risus. Curabitur vitae ultricies tortor. In vulputate eget nisi ac tempor. Ut rhoncus, justo eget scelerisque eleifend, nulla justo blandit justo, non eleifend nisi sapien nec erat.

Cras finibus orci nec lorem semper lobortis. Curabitur blandit metus nec tempor eleifend. Phasellus non odio quam. Sed id orci neque. Vivamus at magna eu tortor venenatis dictum vitae id sapien. Duis vel lobortis turpis, varius semper velit. Mauris ligula leo, aliquet quis libero vitae, consectetur faucibus tellus. Mauris sit amet erat non diam rhoncus congue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eu purus lorem. Phasellus accumsan, justo quis tempor aliquam, magna mauris maximus odio, quis viverra nisi libero eu risus. Donec imperdiet neque porta erat mattis, in ultricies ipsum dignissim. Etiam sit amet ornare eros, eu consectetur odio.

<div class="number2">1.2</div>

## How will this be organized?

Praesent diam lorem, tincidunt quis mollis et, malesuada non diam. Curabitur a hendrerit arcu. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas gravida in massa eget faucibus. Nulla sed venenatis quam. Fusce sagittis ante sed felis gravida porta. Interdum et malesuada fames ac ante ipsum primis in faucibus.

Proin interdum, lacus interdum condimentum facilisis, nunc ante efficitur arcu, id dignissim mauris lorem sed est. Fusce sodales semper enim, in hendrerit risus pharetra tincidunt. Vestibulum at lobortis diam, quis venenatis ex. Aenean hendrerit odio non nibh dignissim feugiat. Sed iaculis iaculis diam. Aenean euismod cursus malesuada. Fusce condimentum lobortis vulputate. Etiam ac purus gravida, porttitor magna id, pretium lectus. Morbi imperdiet metus non massa tincidunt vestibulum. Curabitur eu consequat leo, et facilisis turpis. Mauris sed gravida lectus. Fusce quis quam eget est auctor ullamcorper eget quis sem. Ut nec rhoncus mauris.

Nam fermentum, enim sit amet consequat eleifend, dolor elit laoreet mi, sit amet commodo leo sem tincidunt sem. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer ac fermentum ante. Aliquam in lacus tristique nisi dignissim laoreet vestibulum eu neque. Vivamus orci ipsum, commodo eu quam a, porttitor finibus eros. Quisque hendrerit ornare consequat. Nullam tincidunt at nunc at maximus. Proin facilisis et libero a vulputate. Nam scelerisque felis eu rhoncus iaculis. Fusce eleifend, sem sit amet elementum fringilla, neque ligula tincidunt nisl, sed iaculis magna massa id nulla. Proin tincidunt nunc sit amet tempor aliquet. Sed dignissim, justo nec sagittis vulputate, neque tortor volutpat risus, placerat pretium est nulla iaculis ex. Integer quis tristique risus. Aliquam eget lacinia nibh, sed semper metus.

<div class="number3">1.2.1</div>

### Subsection, h3 here

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla ut rhoncus ex. Praesent vel tempus dolor. Donec id consequat nulla. Aliquam mi metus, pharetra non posuere sed, sollicitudin vitae leo. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent nec tortor quis magna ullamcorper condimentum. Pellentesque tincidunt massa at condimentum posuere. Mauris a tempus risus, nec dictum leo. In feugiat felis sit amet leo laoreet, eget rutrum libero mollis. Maecenas in sem risus. Curabitur vitae ultricies tortor. In vulputate eget nisi ac tempor. Ut rhoncus, justo eget scelerisque eleifend, nulla justo blandit justo, non eleifend nisi sapien nec erat.

Cras finibus orci nec lorem semper lobortis. Curabitur blandit metus nec tempor eleifend. Phasellus non odio quam. Sed id orci neque. Vivamus at magna eu tortor venenatis dictum vitae id sapien. Duis vel lobortis turpis, varius semper velit. Mauris ligula leo, aliquet quis libero vitae, consectetur faucibus tellus. Mauris sit amet erat non diam rhoncus congue. Vestibulum ante ipsum primis in faucibus orci luctus et ultrices posuere cubilia curae; Lorem ipsum dolor sit amet, consectetur adipiscing elit. Curabitur eu purus lorem. Phasellus accumsan, justo quis tempor aliquam, magna mauris maximus odio, quis viverra nisi libero eu risus. Donec imperdiet neque porta erat mattis, in ultricies ipsum dignissim. Etiam sit amet ornare eros, eu consectetur odio.

<div class="number2">1.3</div>

## What steps will we take to implement the language?

Nam fermentum, enim sit amet consequat eleifend, dolor elit laoreet mi, sit amet commodo leo sem tincidunt sem. Orci varius natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Integer ac fermentum ante. Aliquam in lacus tristique nisi dignissim laoreet vestibulum eu neque. Vivamus orci ipsum, commodo eu quam a, porttitor finibus eros. Quisque hendrerit ornare consequat. Nullam tincidunt at nunc at maximus. Proin facilisis et libero a vulputate. Nam scelerisque felis eu rhoncus iaculis. Fusce eleifend, sem sit amet elementum fringilla, neque ligula tincidunt nisl, sed iaculis magna massa id nulla. Proin tincidunt nunc sit amet tempor aliquet. Sed dignissim, justo nec sagittis vulputate, neque tortor volutpat risus, placerat pretium est nulla iaculis ex. Integer quis tristique risus. Aliquam eget lacinia nibh, sed semper metus.

<div class="number3">1.3.1</div>

### Subsection, h3 here

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla ut rhoncus ex. Praesent vel tempus dolor. Donec id consequat nulla. Aliquam mi metus, pharetra non posuere sed, sollicitudin vitae leo. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent nec tortor quis magna ullamcorper condimentum. Pellentesque tincidunt massa at condimentum posuere. Mauris a tempus risus, nec dictum leo. In feugiat felis sit amet leo laoreet, eget rutrum libero mollis. Maecenas in sem risus. Curabitur vitae ultricies tortor. In vulputate eget nisi ac tempor. Ut rhoncus, justo eget scelerisque eleifend, nulla justo blandit justo, non eleifend nisi sapien nec erat.

- Front end: lexer, parser, any necessary analysis
- Interpreter: interpret whatever the front end produces
- Back end: implement a code generator for a few different targets probably

The Hibiku language

Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla ut rhoncus ex. Praesent vel tempus dolor. Donec id consequat nulla. Aliquam mi metus, pharetra non posuere sed, sollicitudin vitae leo. Lorem ipsum dolor sit amet, consectetur adipiscing elit. Praesent nec tortor quis magna ullamcorper condimentum. Pellentesque tincidunt massa at condimentum posuere. Mauris a tempus risus, nec dictum leo. In feugiat felis sit amet leo laoreet, eget rutrum libero mollis. Maecenas in sem risus. Curabitur vitae ultricies tortor. In vulputate eget nisi ac tempor. Ut rhoncus, justo eget scelerisque eleifend, nulla justo blandit justo, non eleifend nisi sapien nec erat.

<div class="challenges">

## Challenges

1.  The lexical grammars of Python and Haskell are not *regular*. What does that
    mean, and why aren't they?

1.  Aside from separating tokens -- distinguishing `print foo` from `printfoo`
    -- spaces aren't used for much in most languages. However, in a couple of
    dark corners, a space *does* affect how code is parsed in CoffeeScript,
    Ruby, and the C preprocessor. Where and what effect does it have in each of
    those languages?

1.  Our scanner here, like most, discards comments and whitespace since those
    aren't needed by the parser. Why might you want to write a scanner that does
    *not* discard those? What would it be useful for?

1.  Add support to Lox's scanner for C-style `/* ... */` block comments. Make
    sure to handle newlines in them. Consider allowing them to nest. Is adding
    support for nesting more work than you expected? Why?

[Test Link](https://github.com/nashiora)

</div>

<div class="design-note">

## Design Note: Implicit Semicolons

Programmers today are spoiled for choice in languages and have gotten picky
about syntax. They want their language to look clean and modern. One bit of
syntactic lichen that almost every new language scrapes off (and some ancient
ones like BASIC never had) is `;` as an explicit statement terminator.

Instead, they treat a newline as a statement terminator where it makes sense to
do so. The "where it makes sense" part is the challenging bit. While *most*
statements are on their own line, sometimes you need to spread a single
statement across a couple of lines. Those intermingled newlines should not be
treated as terminators.

Most of the obvious cases where the newline should be ignored are easy to
detect, but there are a handful of nasty ones:

* A return value on the next line:

    ```js
    if (condition) return
    "value"
    ```

    Is "value" the value being returned, or do we have a `return` statement with
    no value followed by an expression statement containing a string literal?

* A parenthesized expression on the next line:

    ```js
    func
    (parenthesized)
    ```

    Is this a call to `func(parenthesized)`, or two expression statements, one
    for `func` and one for a parenthesized expression?

* A `-` on the next line:

    ```js
    first
    -second
    ```

    Is this `first - second` -- an infix subtraction -- or two expression
    statements, one for `first` and one to negate `second`?

In all of these, either treating the newline as a separator or not would both
produce valid code, but possibly not the code the user wants. Across languages,
there is an unsettling variety of rules used to decide which newlines are
separators. Here are a couple:

*   [Lua](https://www.lua.org/pil/1.1.html) completely ignores newlines, but carefully controls its grammar such
    that no separator between statements is needed at all in most cases. This is
    perfectly legit:

    ```lua
    a = 1 b = 2
    ```

    Lua avoids the `return` problem by requiring a `return` statement to be the
    very last statement in a block. If there is a value after `return` before
    the keyword `end`, it *must* be for the `return`. For the other two cases,
    they allow an explicit `;` and expect users to use that. In practice, that
    almost never happens because there's no point in a parenthesized or unary
    negation expression statement.

*   [Go](https://golang.org/ref/spec#Semicolons) handles newlines in the scanner. If a newline appears following one
    of a handful of token types that are known to potentially end a statement,
    the newline is treated like a semicolon. Otherwise it is ignored. The Go
    team provides a canonical code formatter, [gofmt](https://golang.org/cmd/gofmt/), and the ecosystem is
    fervent about its use, which ensures that idiomatic styled code works well
    with this simple rule.

*   [Python](https://docs.python.org/3.5/reference/lexical_analysis.html#implicit-line-joining) treats all newlines as significant unless an explicit backslash
    is used at the end of a line to continue it to the next line. However,
    newlines anywhere inside a pair of brackets (`()`, `[]`, or `{}`) are
    ignored. Idiomatic style strongly prefers the latter.

    This rule works well for Python because it is a highly statement-oriented
    language. In particular, Python's grammar ensures a statement never appears
    inside an expression. C does the same, but many other languages which have a
    "lambda" or function literal syntax do not.

    An example in JavaScript:

    ```js
    console.log(function() {
      statement();
    });
    ```

    Here, the `console.log()` *expression* contains a function literal which
    in turn contains the *statement* `statement();`.

    Python would need a different set of rules for implicitly joining lines if
    you could get back *into* a <span name="lambda">statement</span> where
    newlines should become meaningful while still nested inside brackets.

<aside name="lambda">

And now you know why Python's `lambda` allows only a single expression body.

</aside>

*   JavaScript's "[automatic semicolon insertion](https://www.ecma-international.org/ecma-262/5.1/#sec-7.9)" rule is the real odd
    one. Where other languages assume most newlines *are* meaningful and only a
    few should be ignored in multi-line statements, JS assumes the opposite. It
    treats all of your newlines as meaningless whitespace *unless* it encounters
    a parse error. If it does, it goes back and tries turning the previous
    newline into a semicolon to get something grammatically valid.

    This design note would turn into a design diatribe if I went into complete
    detail about how that even *works*, much less all the various ways that
    JavaScript's "solution" is a bad idea. It's a mess. JavaScript is the only
    language I know where many style guides demand explicit semicolons after
    every statement even though the language theoretically lets you elide them.

If you're designing a new language, you almost surely *should* avoid an explicit
statement terminator. Programmers are creatures of fashion like other humans, and
semicolons are as passé as ALL CAPS KEYWORDS. Just make sure you pick a set of
rules that make sense for your language's particular grammar and idioms. And
don't do what JavaScript did.

</div>
