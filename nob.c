#define NOB_IMPLEMENTATION
#include "nob.h"

static int rmrf(const char* path) {
    if (nob_get_file_type(path) == NOB_FILE_DIRECTORY) {
        
        Nob_File_Paths file_paths = {0};
        if (!nob_read_entire_dir(path, &file_paths)) {
            return 1;
        }

        for (size_t i = 0; i < file_paths.count; i++) {
            const char* file = file_paths.items[i];
            if (0 == strcmp(file, ".") || 0 == strcmp(file, "..")) {
                continue;
            }
            
            rmrf(nob_temp_sprintf("%s/%s", path, file));
        }
    }

    remove(path);
    return 0;
}

static bool cstring_ends_with(const char* cs, const char* end) {
    size_t cslen = strlen(cs);
    size_t endlen = strlen(end);

    if (cslen < endlen) {
        return false;
    }

    return 0 == strncmp(cs + cslen - endlen, end, endlen);
}

bool process_dir(const char* dir, const char* out, const char* template_start, const char* template_end) {
    nob_mkdir_if_not_exists(out);

    Nob_File_Paths file_paths = {0};
    if (!nob_read_entire_dir(dir, &file_paths)) {
        return 1;
    }

    for (size_t i = 0; i < file_paths.count; i++) {
        const char* file = file_paths.items[i];
        if (0 == strcmp(file, "template.html")) {
            continue;
        }

        const char* real_file = nob_temp_sprintf("%s/%s", dir, file);
        char* out_file = nob_temp_sprintf("%s/%s", out, file);

        Nob_File_Type file_type = nob_get_file_type(real_file);
        
        if (file_type == NOB_FILE_DIRECTORY) {
            if (0 == strcmp(file, ".") || 0 == strcmp(file, "..")) {
                continue;
            }

            process_dir(real_file, out_file, template_start, template_end);
            continue;
        }

        if (cstring_ends_with(file, ".md")) {
            out_file[strlen(out_file) - 3] = 0;

            const char* temp_file = nob_temp_sprintf("%s.tmp.html", out_file);
            out_file = nob_temp_sprintf("%s.html", out_file);

            Nob_Cmd cmd = {0};
            nob_cmd_append(&cmd, "pandoc", real_file, "-o", temp_file);
            if (!nob_cmd_run_sync(cmd)) {
                return false;
            }

            Nob_String_Builder sb = {0};
            if (!nob_read_entire_file(temp_file, &sb)) {
                remove(temp_file);
                nob_sb_free(sb);
                return 1;
            }
            nob_sb_append_null(&sb);
            remove(temp_file);

            const char* templated = nob_temp_sprintf("%s%s%s", template_start, sb.items, template_end);
            nob_sb_free(sb);

            if (!nob_write_entire_file(out_file, templated, strlen(templated))) {
                return 1;
            }
        } else {
            nob_copy_file(real_file, out_file);
        }
    }

    return true;
}

static bool split_template(char* template, const char* flag_string, const char** template_start, const char** template_end) {
    size_t flag_string_len = strlen(flag_string);
    for (size_t i = 0; template[i] != 0; i++) {
        if (0 == strncmp(template + i, flag_string, flag_string_len)) {
            template[i] = 0;
            *template_start = template;
            *template_end = template + i + flag_string_len;
            return true;
        }
    }

    return false;
}

int main(int argc, char** argv) {
    NOB_GO_REBUILD_URSELF(argc, argv);

    const char* template_start = NULL;
    const char* template_end = NULL;

    Nob_String_Builder sb = {0};
    if (!nob_read_entire_file("./src/template.html", &sb)) {
        return 1;
    }
    nob_sb_append_null(&sb);

    char* template_source = sb.items;
    if (!split_template(template_source, "$$body$$", &template_start, &template_end)) {
        return 1;
    }

    rmrf("./public_html");
    if (!process_dir("./src", "./public_html", template_start, template_end)) {
        return 1;
    }

    return 0;
}
