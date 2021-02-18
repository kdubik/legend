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
    success = False
    print("Creating backup...")

    file_name = Path(f"../legend_history/legend_v{sys.argv[1]}.zip")
    if not (os.path.exists(file_name)):
        zf = zipfile.ZipFile(file_name, "w")
        for dirname, _, files in os.walk("."):

            if check_unnecessary_folders(dirname):
                if dirname!='.': zf.write(dirname)
                for filename in files:
                    zf.write(os.path.join(dirname, filename))
        
        zf.close()
        success = True
    else:
        print("Error: File already exist! Probably bad version number is used as parameter!")

    return success

def upload_files_to_git():
    print("Uploading files to GIT...")

    cmd = "git add ."
    #print (cmd)
    os.system(cmd)

    cmd = f'git commit -m "v{sys.argv[1]}"'
    #print (cmd)
    os.system(cmd)

    cmd = "git push"
    #print (cmd)
    os.system(cmd)

if __name__ == '__main__':
    show_header()

    if len(sys.argv) > 0:
        backup_completed = backup_files()
        if backup_completed==True: upload_files_to_git()
    else:
        print("Error: not enough input parameters")
