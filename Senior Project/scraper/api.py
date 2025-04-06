from flask import Flask, jsonify
import pandas as pd
import os

app = Flask(__name__)

CSV_FILE_PATH = r"C:\Users\bilgi\OneDrive\Masaüstü\Senior Project\scraper\enriched_urunler.csv"

# CSV dosyasını oku ve JSON'a dönüştür
def load_products():
    try:
        df = pd.read_csv(CSV_FILE_PATH, encoding="utf-8-sig")
        return df.to_dict(orient="records")  # DataFrame'i JSON formatına çevir
    except FileNotFoundError:
        return {"error": "CSV dosyası bulunamadı. Önce scraping yapmalısınız!"}

@app.route('/get-products', methods=['GET'])
def get_products():
    products = load_products()
    return jsonify(products)

if __name__ == '__main__':
    app.run(port=5000, debug=True)
