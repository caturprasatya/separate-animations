import json
import os

def collecting_animations(directory):
    data = []
    
    for filename in os.listdir(directory):
        data.append("<S3_URL>"+filename)
    
    return data

def save_to_json(data, filename):
    try:
        with open(filename, "w") as json_file:
            json.dump(data, json_file, indent=4)
            print(f"Data successfully saved to {filename}")
    except (TypeError, IOError) as e:
        print(f"Error saving to JSON file: {e}")


if __name__ == "__main__":
    print("Running code")
    meta = {
        "version": "1.0.0",
    }
    filename = "metadata.json"
    directory = r"<DIRECTORY>"

    s3UrlAnimations = collecting_animations(directory)

    # Update the meta dictionary with the new data
    metadata = meta.copy()  # Make a copy to avoid modifying the original meta
    metadata.update({"data": s3UrlAnimations})

    save_to_json(metadata, filename)

