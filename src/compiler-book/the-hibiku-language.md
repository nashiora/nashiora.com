<div class="number1">$chapter$</div>

# The Hibiku Language

<aside>

There's often confusion about the terms used to describe a language's type
system. The way I've come to understand it is there are two main axes we refer
to: static vs. dynamic, and strong vs. weak.

A static type system is one which is resolvable at compile time, meaning you
know the type of a variable for certain before you even run the program, and a
dynamic type system makes no guarantee of this: any variable could be any type
at any time!

A strong type system does not allow implicit conversions between
types or allow values of one type to easily be assigned to variables which
expect another, while a weak type system much more freely allows this.

</aside>

The language we'll be building a compiler for in this book is called Hibiku, a
static and strongly typed language with a relatively minimal feature set. Though
it may be a small language, it's still general purpose and fairly feature dense!
It'd be hard to teach about compilers if the reference language only covered a
small portion of real world features, but we also have to keep in mind a book
can't go on forever.

Hibiku supports a handful of built-in primitive types along with functions,
closures, structs and enums.

<div class="design-note">

When creating your own programming language, or any project for that matter,
don't think too hard about the name! For example, I named Hibiku after scrolling
through some of my favorite songs and tweaking their names a bit until I had
something short and unique. Ideally you'd also make the name easy to remember
and say for as many people as possible.

Hibiku is named after the Japanese verb 響く (pronounced the same way) meaning "to
resound" or, as the song title is often translated as, "to resonate". I
sincerely hope this book resonates with you, or that you at least find it a good
reference material.

</div>
