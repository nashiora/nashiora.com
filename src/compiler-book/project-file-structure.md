<div class="number1">$chapter$</div>

# File Structure

If you're following along or thumbing through the reference implementation for
Hibiku, it'll be helpful to understand the project's file structure.

Hibiku is designed to be used both as a library and as an executable
compiler/interpreter. To easily fascilitate this, the source code is split into
three directories.
- The `include` directory contains the public header files (`.h`) that define
  the API for using Hibiku as a library. If you've never used C or C++ before,
  then header files might be foreign to you. I'll explain what we're putting in
  each header file and why in later sections and briefly talk about what header
  files actually are, but you may still want to do your own research.
  - The primary public API for Hibiku is found in `include/hibiku.h`. Any
    application consuming Hibiku as a library will include this file.
- The `lib` directory contains all of the source code necessary to build Hibiku
  as a library. Private header files (`.h`) provide a consistent internal API
  and source files (`.c`) provide the implementation of the library, both the
  internal and external APIs and any miscellaneous supporting code.
- The `src` directory contains only the code necessary to build Hibiku as a
  standalone compiler/interpreter executable. The source code in this directory
  treats Hibiku exclusively as a library.
  - The entry point for the compiler/interpreter executable is `src/hibiku.c`.
    The program's `main` function and command line utilities exist here.
