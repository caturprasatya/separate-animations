import json

def save_to_json(data, filename):
    try:
        with open(filename, "w") as json_file:
            json.dump(data, json_file, indent=4)
            print(f"Data successfully saved to ${filename}")
    except (TypeError, IOError) as e:
        print(f"Error saving to JSON file: {e}")


if __name__ == "__main__":
    print("Running code")
    meta = {
        "version": "1.0.0",
        "data": []
    }
    filename = "metadata.json"

    
