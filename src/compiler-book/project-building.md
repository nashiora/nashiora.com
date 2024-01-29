<div class="number1">$chapter$</div>

# Building the Project

Let's get the project set up, a simple "Hello, world" to make sure you can build
C code correctly and automatically.

First, create the `src` directory in your project, then create `src/hibiku.c`.

```c
// src/hibiku.c
#include <stdio.h>

int main() {
    printf(stderr, "Hello, Hibiku!\n");
    return 0;
}
```

This is nothing more than a simple program to print to the console and make sure
your C compiler works. This example uses the Clang C compiler, but any standard
compliant C compiler should do the trick.

```sh
$ clang -o hibiku src/hibiku.c
$ ./hibiku
Hello, Hibiku!
```

Once that works, create the `include` directory, then create `include/hibiku.h`.

```c
// include/hibiku.h
#ifndef HIBIKU_H
#define HIBIKU_H

#define HIBIKU_VERSION "Hibiku 1.0"

#endif
```

Then update `src/hibiku.c` to use this new header file.

```c
// src/hibiku.c
#include <stdio.h>

#include <hibiku.h>

int main() {
    printf(stderr, "Hello, %s!\n", HIBIKU_VERSION);
    return 0;
}
```

And compile one more time, making sure to tell your compiler about the new include directory.

```sh
$ clang -o hibiku -I include src/hibiku.c
$ ./hibiku
Hello, Hibiku 1.0!
```
