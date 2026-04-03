import pandas as pd
url = "https://docs.google.com/spreadsheets/d/1eaWpt3_zU3CPtfcho7fCUc5yhVg2fpSJadoqGkrKlUM/export?format=csv&gid=15514277"
try:
    df = pd.read_csv(url)
    with open("requirements.txt", "w", encoding="utf-8") as f:
        for index, row in df.iterrows():
            f.write(f"--- Req {row.get('Requirement ID', '')} ({row.get('Title', '')}) ---\n")
            f.write(f"Screen: {row.get('Screen ID', '')}\n")
            f.write(f"Desc: {row.get('Description', '')}\n")
            f.write(f"AC: {row.get('Acceptance Criteria', '')}\n\n")
    print("Requirements saved to requirements.txt")
except Exception as e:
    print("Failed to read:", e)
