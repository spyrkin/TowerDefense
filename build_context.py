#!/usr/bin/env python3
"""
Script to prepare hyper-context.
Recursively traverses a directory, builds the file structure, and collects
the contents of files (excluding build_context.py) into an output file hyper_context.txt

Usage:
    python build_context.py [path_to_directory]

If the path is not specified, the current directory is used.
"""

import os
import re
import sys

SCRIPT_NAME = os.path.basename(sys.argv[0])

# ──────────────────────────────────────────────
#  Parsing .gitignore
# ──────────────────────────────────────────────


def parse_gitignore(root: str) -> tuple[list[str], list[str]]:
    """Reads .gitignore and returns (ignore_patterns, exception_patterns)."""
    gitignore_path = os.path.join(root, ".gitignore")
    ignore: list[str] = []
    exceptions: list[str] = []

    if not os.path.isfile(gitignore_path):
        return ignore, exceptions

    with open(gitignore_path, "r", encoding="utf-8") as fh:
        for line in fh:
            pattern = line.strip()
            if not pattern or pattern.startswith("#"):
                continue

            if pattern.startswith("!") and not pattern.startswith("!!"):
                exceptions.append(_glob_to_regex(pattern[1:]))
            else:
                ignore.append(_glob_to_regex(pattern))

    print(f"[.gitignore] ignore: {len(ignore)}, exceptions: {len(exceptions)}")
    return ignore, exceptions


def _glob_to_regex(pattern: str) -> str:
    """Converts a gitignore glob pattern to regex."""
    # Remove trailing separator to search for the name without the slash
    pattern = pattern.rstrip("/\\")
    pattern = pattern.replace("/", os.sep)
    regex = re.escape(pattern)
    # Convert escaped glob special characters to corresponding regex
    regex = regex.replace(r"\[", "[").replace(r"\]", "]")
    regex = regex.replace(r"\*", ".*").replace(r"\?", ".+")
    return regex


def is_ignored(rel_path: str, ignore: list[str], exceptions: list[str]) -> bool:
    """Checks if the relative path matches any ignore rule."""
    for p in exceptions:
        if re.search(p, rel_path):
            return False
    for p in ignore:
        if re.search(p, rel_path):
            return True
    return False


# ──────────────────────────────────────────────
#  Collecting entries (directories + files)
# ──────────────────────────────────────────────

EXCLUDED_EXTENSIONS = {".pyd", ".pyc", ".pyo", ".db"}
EXCLUDED_FOLDERS = {".ai", ".git"}
OUTPUT_FILENAME = "hyper_context.txt"


def collect_entries(
    root: str,
    ignore: list[str],
    exceptions: list[str],
) -> list[tuple[str, bool]]:
    """
    Traverses the directory tree and returns a list of tuples
    (relative_path, is_file) in depth-first order.
    """
    entries: list[tuple[str, bool]] = []

    for dirpath, dirnames, filenames in os.walk(root, topdown=True):
        rel_dir = os.path.relpath(dirpath, root)
        if rel_dir == ".":
            rel_dir = ""

        # Filter subdirectories in-place so os.walk doesn't descend into ignored ones
        dirnames[:] = sorted(
            d
            for d in dirnames
            if d not in EXCLUDED_FOLDERS
            and not is_ignored(
                os.path.join(rel_dir, d) if rel_dir else d,
                ignore,
                exceptions,
            )
        )

        # Add directories
        for d in dirnames:
            rel = os.path.join(rel_dir, d) if rel_dir else d
            entries.append((rel, False))

        # Add files
        for f in sorted(filenames):
            # Skip the output file itself
            if f == OUTPUT_FILENAME or f == SCRIPT_NAME or f == "LICENSE":
                continue
            # Skip compiled Python files
            if os.path.splitext(f)[1].lower() in EXCLUDED_EXTENSIONS:
                continue

            rel = os.path.join(rel_dir, f) if rel_dir else f
            if is_ignored(rel, ignore, exceptions):
                continue

            entries.append((rel, True))

    return entries


# ──────────────────────────────────────────────
#  Formatting the output file
# ──────────────────────────────────────────────


def build_output(root: str, entries: list[tuple[str, bool]], output_path: str) -> None:
    project_name = os.path.basename(root)

    with open(output_path, "w", encoding="utf-8") as out:
        # ── Header ──
        out.write(f"I have a project {project_name} with following file structure\n\n")

        # ── Structure section ──
        for rel_path, _ in entries:
            out.write(f"{rel_path}\n")

        out.write("\n")

        # ── File contents section ──
        for rel_path, is_file in entries:
            if not is_file:
                continue

            full_path = os.path.join(root, rel_path)

            try:
                content = _read_file(full_path)
            except (UnicodeDecodeError, PermissionError, OSError):
                # Skip binary / inaccessible files
                continue

            if content is None:
                continue

            out.write(f"\n~~~{rel_path}\n")
            out.write(content)
            if not content.endswith("\n"):
                out.write("\n")
            out.write("~~~\n")


def _read_file(path: str) -> str | None:
    """Attempts to read the file as UTF-8; returns None on failure."""
    try:
        with open(path, "r", encoding="utf-8") as fh:
            return fh.read()
    except UnicodeDecodeError:
        pass
    # Fallback: try cp1251 (common encoding in 1C exports)
    try:
        with open(path, "r", encoding="cp1251") as fh:
            return fh.read()
    except (UnicodeDecodeError, OSError):
        return None


# ──────────────────────────────────────────────
#  Entry point
# ──────────────────────────────────────────────


def main() -> None:
    # Determine the root directory
    if len(sys.argv) > 1 and sys.argv[1] != ".":
        root = os.path.abspath(sys.argv[1])
    else:
        root = os.getcwd()

    if not os.path.isdir(root):
        print(f"Error: directory not found — {root}", file=sys.stderr)
        sys.exit(1)

    output_path = os.path.join(root, OUTPUT_FILENAME)

    print(f"Root   : {root}")
    print(f"Output : {output_path}")

    ignore, exceptions = parse_gitignore(root)
    entries = collect_entries(root, ignore, exceptions)

    dirs_count = sum(1 for _, is_f in entries if not is_f)
    files_count = sum(1 for _, is_f in entries if is_f)
    print(f"Entries: {dirs_count} dirs, {files_count} files")

    build_output(root, entries, output_path)

    size_kb = os.path.getsize(output_path) / 1024
    print(f"Done. {OUTPUT_FILENAME} written ({size_kb:.1f} KB)")


if __name__ == "__main__":
    main()
