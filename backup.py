#!/usr/bin/python3

import sys
import zipfile
import os
from pathlib import Path

# List of expluded folders
LIST_OF_FOLDERS = ['/obj','/bin','/.git']

def show_header():
    print('Legend RPG game backup script, v0.1 (21.Jan 2021)')
    print('by Kamil Dubik\n')

    print ('Number of arguments:', len(sys.argv), 'arguments.')
    if len(sys.argv)>0:
        print ('Arguments:', str(sys.argv[1]))

def check_unnecessary_folders(folder_path):
    res = True
    for fname in LIST_OF_FOLDERS:
        if fname in folder_path: res = False
    return res

def backup_files():
    print("Creating backup...")

    zf = zipfile.ZipFile(Path(f"../legend_history/legend_v{sys.argv[1]}.zip"), "w")
    for dirname, _, files in os.walk("."):

        if check_unnecessary_folders(dirname):
            if dirname!='.': zf.write(dirname)
            for filename in files:
                zf.write(os.path.join(dirname, filename))
     
    zf.close()

if __name__ == '__main__':
    show_header()

    if len(sys.argv) > 0:
        backup_files()
    else:
        print("Error: not enough input parameters")
