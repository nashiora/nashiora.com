.DEFAULT_GOAL := all

SRC_LAYE_DOCS = ./src/laye/docs/
OUT_LAYE_DOCS = ./public_html/laye/docs/

OUT_LAYE_DOCS_FILES = $(patsubst $(SRC_LAYE_DOCS)%.adoc, $(OUT_LAYE_DOCS)%.html, $(wildcard $(SRC_LAYE_DOCS)*.adoc))

all: compiler-book laye

compiler-book: ./public_html/compiler-book.html

laye: laye-docs

./public_html/compiler-book.html: ./src/compiler-book/main.adoc
	asciidoctor -o $@ $<

laye-docs: $(OUT_LAYE_DOCS_FILES)

$(OUT_LAYE_DOCS)%.html: $(SRC_LAYE_DOCS)%.adoc
	asciidoctor -o $@ $<

publish:
	./publish.sh

clean:
	rm -r public_html
