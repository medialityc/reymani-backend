import os

def rename_files_in_directory():
    current_directory = os.getcwd()
    
    for filename in os.listdir(current_directory):
        if "Cliente" in filename:
            new_filename = filename.replace("Cliente", "Usuario")
            old_path = os.path.join(current_directory, filename)
            new_path = os.path.join(current_directory, new_filename)
            
            try:
                os.rename(old_path, new_path)
                print(f'Renombrado: "{filename}" -> "{new_filename}"')
            except Exception as e:
                print(f'Error renombrando "{filename}": {e}')

if __name__ == "__main__":
    rename_files_in_directory()
