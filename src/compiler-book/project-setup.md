<div class="number1">$section$</div>

# Project Setup

*If you don't intend to follow along with the book and write your own code, or
aren't concerned with reading through the reference implementation of Hibiku,
this section can safely be skipped.*

<hr />

The reference implementation of Hibiku is written in the C programming language.
While the primary goal of this book is to educate on the construction of
compilers in a language-agnostic way, the code samples and reference compiler
needed to be implemented in an existing language. C was chosen for its relative
simplicity, and for my familiarity with it.

Despite its simplicity, or rather due to it, C does not provide some common data
structures you'd find in a modern general purpose programming language. Part of
the project setup section will detail necessary prerequisite data structures
that aren't provided by C or its standard library.

While I'd love to teach you everything I know about C, this is not the place for
that. You aren't going to need years of experience to understand the reference
code, and advanced topics will be introduced with enough description and proper
language to fascilitate your own research at your own pace.
