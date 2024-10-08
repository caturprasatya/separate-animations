import json
import os
from dotenv import load_dotenv

def collection_animation(directory):
    words = []

    for filename in os.listdir(directory):
        if filename.endswith(".anim"):
            trigerName = filename.split(".")[0]
            words.append(trigerName)

    return words

def save_to_json(data, filename):
    try:
        with open(filename, "w") as json_file:
            json.dump(data, json_file, indent=4)
            print(f"Data successfully saved to {filename}")
    except (TypeError, IOError) as e:
        print(f"Error saving to JSON file: {e}")

def read_json_file(filename):
    try:
        with open(filename, "r") as json_file:
            data = json.load(json_file)

            return data
    except (TypeError, IOError) as e:
        print(f"Error read JSON file: {e}")
        return;

def find_word(animation_names, dictionary):
    data = []

    for word in dictionary:
        labels = [label.lower() for label in str(word.get("labels", " ")).split(", ")]
        for name in animation_names:
            if name in labels:
                synonim = {
                    "triggerName": name,
                    "synonims": [label for label in labels if(label != name)]
                }

                data.append(synonim)

    return data


if __name__ == "__main__":
    load_dotenv()

    sign_language = {
        "animationEntries": []
    }

    output_filename = "words.json"
    dictionary_filename = "dictionary-words.json"
    directory = os.environ.get("DIRECTORY")

    animation_words = collection_animation(directory)
    dictionaries = read_json_file(dictionary_filename) 

    animation_entries = find_word(animation_words, dictionaries)

    sign_language["animationEntries"] = animation_entries

    save_to_json(sign_language, output_filename)


