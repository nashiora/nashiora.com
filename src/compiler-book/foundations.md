# Foundations

Before the train gets rolling and we start handling source code, we need to set
up some data structures that C doesn't provide in its standard library. Most
modern languages will ship with all of the data structures considered necessary
for writing code in the 21st century, but C asks that we do so ourselves. We're
going to need the following additional primitive types to get our compiler
implementation off the ground: a dynamic array, a hash table, an owned and
mutable string, an unowned string view and a pooling memory allocator.

If you'd prefer to get your hands on off the shelf versions of these, here
are my recommendations for open source libraries:

- Dynamic Arrays
    - Sean T. Barrett's popular STB single-header libraries, specifically `stb_ds.h`.
- Hash Tables
    - Sean T. Barrett's popular STB single-header libraries, specifically `stb_ds.h`.
- Strings
    - Salvatore Sanfilippo's SDS (Simple Dynamic Strings) library.
    - Implement your own as a thin wrapper over the dynamic arrays in the `stb_ds.h` library from Sean T. Barrett.

For any string library that doesn't give you string views, you can implement
them trivially over any string with a structure like the following:

```c
typedef struct {
    // get this data from wherever you need,
    // or embed the type of the string you're wrapping.
    char* string_data;
    // the length of this string view so we know
    // exactly how many characters we care about.
    int64_t length;
    // you only need an explicit offset if you can't
    // store the data in a manner tha precomputes it.
    // requiring an offset field also means you should
    // provide helper functions for accessing the
    // underlying memory, like:
    //   `sv_char_at(my_string_view sv, int64_t index)`
    int64_t offset;
} my_string_view;
```

The book will assume a string view implementation akin to the above, without
requiring the offset field, to simplify its use in things like `printf`. More
information on the string view implementation and usage can be found in the
[chapter on strings](strings.html).

This unit will also provide a high level structure for the entire compiler, end
to end, so we can prepare some work ahead of time. Things like command line
argument parsing, shared compiler state, handling source storage in a file
agnostic manner, etc.
