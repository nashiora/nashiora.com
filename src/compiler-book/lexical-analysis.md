<div class="number1">$section$</div>

# Lexical Analysis

<aside>"Lexical" refers to the words and vocabulary of a language. For
programming languages, we tend to include symbols and punctuation in our
definition. </aside>

The first stage of any compiler is lexical analysis. We need some way to make
sense out of what is otherwise just plain text. The way most compilers achieve
this is by first turning the input source text into a series of lexical tokens,
then giving those tokens meaning by building a syntax tree.

<aside class="down2">"Lexer" is short for "lexical analyzer". You can also call
it a "tokenizer" or "scanner" if you prefer.</aside>

There are many different ways to go about lexical analysis in a compiler, but
this book will focus primarily on doing the work by hand. We'll be building our
own lexer and parser. The lexer is responsible for producing the aforementioned
sequence of tokens, and the parser is responsible for getting high structured
meaning out of them.

In the next chapter we'll kickstart our compiler journey by building a lexer for
Hibiku.
