import json
import os
from dotenv import load_dotenv

def collecting_animations(directory):
    data = []

    for filename in os.listdir(directory):
        if filename.endswith(".anim"):
            file = {
                "triggerName": filename,
                "S3Url": os.environ.get("S3_URL") + filename,
                "synonim": []
            }
            data.append(file)
    
    return data

def save_to_json(data, filename):
    try:
        with open(filename, "w") as json_file:
            json.dump(data, json_file, indent=4)
            print(f"Data successfully saved to {filename}")
    except (TypeError, IOError) as e:
        print(f"Error saving to JSON file: {e}")


if __name__ == "__main__":
    load_dotenv()
    print("Running code")
    meta = {
        "version": "1.0.0",
    }
    filename = "metadata.json"
    directory = os.environ.get('DIRECTORY')

    print(os.environ.get("S3_URL"))

    s3UrlAnimations = collecting_animations(directory)

    # Update the meta dictionary with the new data
    metadata = meta.copy()  # Make a copy to avoid modifying the original meta
    metadata.update({"data": s3UrlAnimations})

    save_to_json(metadata, filename)

