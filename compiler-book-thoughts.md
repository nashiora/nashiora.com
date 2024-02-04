
Some points I need to keep in mind:

## Market research


- Is there a market for the textbook: are other people teaching similar courses?
  Is this course taught at universities around the world?

  My streams and discussions in the Discord prove there is a market for an
  approachable textbook on compilers. The only reference I know at the moment
  that's in a similar position is Crafting Interpreters, which I'm trying not to
  rip off. I don't know what's taught in universities, but I also don't think
  it should be my priority to think about formal education environments first.
  Afterthough, most likely.

- What should be covered in the textbook: look at how other instructors teach
  this course - what topics are commonly taught? These should feature in your
  textbook.

  Here's where it's okay to look at Crafting Interpreters, but where I should
  also get my hands on other references. I know, high level, everything I want
  to teach in the book. I should probably write it all down.

- How should you structure the textbook: again, looking how this course is
  taught, is there a common order that the topics are taught? Your textbook
  should reflect this.

  I have the high level idea for a few different sections going through the
  entire compiler writing process roughly linearly. I want to include things
  that aren't strictly about the core compiler process, like debugging the
  different steps in the compiler or meta decisions about the design of the
  language being compiled.

- What features to competing textbooks include: If they all have exercises then
  yours probably should too. Is there anything that you could add to your
  textbook to make it stand out from the others, e.g. case studies, definitions
  of key terms, etc.?

  I don't have enough of a sample size currently. Crafting Interpreters includes
  challenges and footnotes to topics discussed in the chapter, as well as asides
  in the margin. I tentatively added a third inline category, "tangents",
  thinking that an aside might be too small but a footnote too late. I don't yet
  know what else, besides the hope that it's beginner friendly, I can do to
  stand out just yet.

- Look at reviews of competing textbooks: what do readers like/dislike about the
  textbook? Have a look at sources such as Amazon and speak to colleagues about
  the textbooks they use.

  I should do this!

  I definitely want to cover optimizations. "Also lacking in this book is a good
  description of advanced optimization techniques and modern intermediate
  representations." for example.

  Also of note, "the book addresses many of the engineering concerns with
  writing a compiler, such as data structures or memory management strategies.
  This is cool." I should probably put more time into the actual setup work
  of the compiler than I currently am since we're working in C. I kinda glossed
  over some things that should probably be more detailed and referenceable.
  Like dynamic arrays, strings, allocators.

  Someone reviewing Crafting Interpreters has similar thoughts to me, that the
  Java tree-walk interpreter is not the most useful. It could be cut. I don't
  plan to implement an entire tree-walk interpreter, but there will be constant
  folding and eventually on-demand byte code execution, so I'll have plenty
  of space to talk about it organically as opposed to CI's entire section
  and implementation dedicated to it.

  The same reviewer found the code snippets disorienting. The entire Lox
  implementation source code exists in the book, fragmented but annotated with
  where changes occur and what places the code lives in the source. This helps,
  but the larger issue is the constant switch from English to source code,
  accoring to them.

  They suggest longer detailed English explanations and less thorough code
  examples, with the Git repo supplementing and probably including tags for
  chapter contents. At this point I've already gone through a good few chapters
  worth of content in my repository, and I don't think I'm nearly prepared
  enough to do this anyway, so I'll keep the plan of using the Git repo solely
  as a resource and not plan to integrate it verbatim.

  I also agree that the manner of implementation, while necessary to write the
  book how he needed to, doesn't always make a lot of sense. Free singletons,
  short functions for the sake of inline code in the book, terse names, avoiding
  passing arguments to functions, etc. all make the code a bit harder to read.


## Preparing the textbook (proposal, but for myself mostly)

- Who am I writing this textbook for: have a clear understanding of who your
  target audience is i.e. what level of degree course will this textbook
  support?

  I want to write for relatively experienced programmers so I can dive fully
  into all of the complications of compilers. Ideally I also write for people
  who have never once looked into how compilers actually work, or at least
  assume some very bare minimum information at the outskirts.

  I'd love for this to function both as an introductory book, "baby's first
  compiler" but also a thorough reference that, while not fit for the top
  requirements of compilers, gets the job done for more than just the easy bits.
  If that makes any sense. A competent book that doesn't assume much about your
  present understanding of compilers.

- What is the objective of my textbook: Why is this textbook needed? Will it be
  a core course textbook, i.e. the only textbook for the course, or will it be
  more supplementary i.e. only covering part of a course and appearing on a
  recommended reading list? How will it meet a course curriculum?

  Not necessarily thinking course material, of course, but this should be a core
  material: you only need this book to do everything.

- How will students benefit from my textbook: will they gain an in-depth
  understanding of a topic, or develop a skill set to understand a particular
  problem, etc.?

- Do I have already material that I can turn into a manuscript: can I repurpose
  my own lecture notes, slides, assignments/course questions, etc.?

  Not really, no...

## Tips for writing it

- Prerequisite knowledge: what topics or concepts should readers already be
  familiar with? Do you need to review these or further explain them?

  I mention to my self all the time that I need to be
  aware of exactly what prerequisite knowledge I'm expecting of the reader. I
  want to write something that is approachable for those new to compiler dev,
  I know that for sure, but I can't assume they're truly novice *programmers*.

- Self-contained: students typically want a one-stop resource so you should try
  to ensure that as much of the information that student needs is presented in
  your textbook.

  I know I want to be a resource, so anything I plan to
  teach in the book needs to be available in the book. This doesn't mean also
  teaching C! Maybe I write C educational resources as well, but this is not
  that.
  
- Modular chapters: students will likely dip in and out of the textbook rather
  than read it linearly from start to finish so try to make chapters
  self-contained where possible, so they can be understood out of context of the
  rest of the textbook.

- Succinct and to the point: keep focused on the course that the textbook is
  supporting and the topics that need to be covered. Avoid including less
  relevant topics, very advanced topics, explanations of concepts that students
  should already understand, and any other content which may not actually be
  useful to the student.

- Didactic elements: elements such as exercises, case studies, definitions and
  so on help break up the main chapter text and make it more engaging. Consider
  what didactic elements you want to include before you start writing so you can
  ensure that the main chapter text provides the right information to support
  the didactic element e.g. that a concept is adequately explained in order to
  answer an exercise question, or that theory is suitably described before a
  corresponding case study is given.

- Writing style: textbooks can have a lighter, more conversational writing style
  than monographs and references works. Try to use active rather than passive
  sentences e.g. “It is believed by some physicians that…” becomes “Some
  physicians believe that…”.

- Online resources: if you have exercises, consider writing a solutions manual
  for instructors so they don’t have to work out all the solutions themselves.
  Are there data sets, spreadsheets, programs, etc., that would be useful for
  students to access so they can test concepts themselves? The same copyright
  issues apply for online resources as for the print book – see Obtaining
  permissions for further information.

- Write an Introduction to explain who the textbook is for and how it should be
  used: confirm the level of the students e.g. 3rd year undergraduates; confirm
  the course that the textbook supports; list any prerequisites or assumptions
  you have made about the student’s background knowledge; explain how the
  textbook could be used. If applicable, identify core must-read chapters and
  chapters which are more advanced or optional; provide short summaries of the
  chapters (just a sentence or two).

- Test your material as you write: use your draft chapters as part of your
  lecture course and see how students respond to it. Do they understand the
  concepts you are explaining? Are they able to complete any exercises?.
