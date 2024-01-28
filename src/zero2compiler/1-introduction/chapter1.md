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
