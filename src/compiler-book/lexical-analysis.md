<div class="number1">$chapter$</div>

# Lexical Analysis

Individual characters in a language don't existin a vaccuum; they combine into
words and phrases, group and delimit sections, and modify other constructions.
To make sense of source code, we first need to define those rules for our
language.

Look at the following source code and see if any rules jump out at you.

```hibiku
function add(left, right)
    return left + right;
end
```

Before getting into what this means in the context of a programming language, we
as humans can already read this and begin to intuit things about it. There are
words separated by spaces, and there are symbols grouping words visually.
Knowing this is a programming language, we see a `function` called `add` that
accepts a `left` and `right` value. This function `return`s their sum and is
closed by the word `end`. The `left` and `right` input parameters are enclosed
within `(` and `)` and separated by `,` as you would a list.

We've just identified the smallest relevant parts of the language. The word
`function` loses its meaning if you remove any of its characters or break it
apart and the parenthesis don't combine with the words around them. By
convention we call these tokens, fundamental pieces of the language which cannot
be broken down further.

## Understanding Tokens

Each token is of a single *kind* that can be more easily reasoned about and
referenced. For example, the code `34 + 35` is three tokens: `INTEGER`, `PLUS`,
and `INTEGER`. Any unbroken series of decimal digits is an `INTEGER` token and
the `PLUS` token stands alone.

Names like `add` or `left` are called identifiers while names with special
meaning like `function` or `return` are called keywords. Since keywords are
always same exist in a relatively small quantity, we can use unique token kinds
for them such as `RETURN`. So `return left + right` may consist of the tokens
`RETURN`, `IDENTIFIER`, `PLUS`, and `IDENTIFIER`.

Notice how integers and identifiers don't have unique token kinds. Since a name
could be any sequence of letters, aside from those designated as keywords, and
numbers are similarly infinite, there's no way to define them as unique token
kinds. Instead we'll have to keep track of their unique meaning in addition to
what kind they are. So while a keyword token only requires its kind, an
identifier requires its kind *and its name*.

```c
typedef enum {
    HBK_TOKEN_IDENTIFIER,
    HBK_TOKEN_INTEGER,
    HBK_TOKEN_RETURN,
    // more token kinds...
} hbk_token_kind;

typedef struct {
    hbk_token_kind kind;
    int64_t integer_value;
    hbk_string_view string_value;
} hbk_token;
```

<aside name="hbk_token_kind">

Sadly, I won't be explaining much of how programming in C actually works in this
book, but I will do my best to help you along in your own research. Due to the
way C creates type names, we need to use `typedef` to actually be able to use
the names `hbk_token_kind` or `hbk_token` as types.

</aside>

To give the token kinds names we can use throughout our compiler, we put them
in an enum. To store the information we need for our token, we create a struct
with a `kind` of that enum type as well as the additional information for
tokens with varying values.

<div class="tangent">

### Tangent: Strings in C

To make our lives easier, we'll be using a "string view" to handle strings in C.
A string view is a reference to the actually string data plus a length telling
us how much of that string is actually important. This allows us to pick
arbitrary parts of a string to look at without having to create copies of it.
This is why it's called a view; we don't own the string or copy it, we just view
it.

```c
typedef struct {
    const char* data;
    int64_t length;
} hbk_string_view;
```

If we care about the word `add` in the string `"function add(left, right)"`, we can
create a string view pointing 9 characters past the start with a length of 3.

</div>
