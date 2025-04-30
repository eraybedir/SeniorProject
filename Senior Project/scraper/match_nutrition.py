import pandas as pd
import json
import time
import re

# Dosya yolları
CSV_INPUT = "products_new.csv"
CSV_NUTRITION = "besin_degerleri.csv"
OUTPUT_CSV = "enriched_urunler_new.csv"
OUTPUT_JSON = "enriched_urunler_new.json"

# 🔎 Besin değerleri CSV'yi yükle
def load_nutrition_reference(file_path):
    df = pd.read_csv(file_path)
    nutrition_map = {}
    for _, row in df.iterrows():
        nutrition_map[row["AnahtarKelime"].lower()] = {
            "CaloriesPer100g": row["CaloriesPer100g"],
            "ProteinPer100g": row["ProteinPer100g"],
            "CarbsPer100g": row["CarbsPer100g"],
            "FatPer100g": row["FatPer100g"]
        }
    return nutrition_map

#  Tam kelime eşleşmesini kontrol eden fonksiyon
def keyword_in_name(product_name, keyword):
    return re.search(rf"\b{re.escape(keyword)}\b", product_name.lower()) is not None

#  Ürün ismine göre besin değeri getir
def get_nutrition_from_keyword(product_name, nutrition_map):
    product_name = product_name.lower()
    
    # En uzun keyword’lerden başlayarak sırala
    for keyword in sorted(nutrition_map, key=len, reverse=True):
        if keyword_in_name(product_name, keyword):
            return nutrition_map[keyword]
    
    # Eşleşme bulunamazsa None dön
    return {
        "CaloriesPer100g": None,
        "ProteinPer100g": None,
        "CarbsPer100g": None,
        "FatPer100g": None
    }

#  Ürünleri besin bilgileriyle zenginleştir
def enrich_all_products():
    df = pd.read_csv(CSV_INPUT)
    nutrition_ref = load_nutrition_reference(CSV_NUTRITION)
    enriched = []

    print(f" {len(df)} ürün işleniyor...\n")

    for i, row in df.iterrows():
        name = row["Ürün İsmi"]
        nutrition = get_nutrition_from_keyword(name, nutrition_ref)

        enriched_product = {
            "Name": name,
            "Market": row["Market"],
            "Price": row["Fiyat"],
            "CategoryName": row["Kategori"],
            "ImageUrl": row["ImageUrl"],  # ✅ Burayı ekliyoruz
            **nutrition
        }

        enriched.append(enriched_product)
        print(f"✅ {i+1}. ürün işlendi → {name}")
        time.sleep(0.05)

    # CSV'ye kaydet
    pd.DataFrame(enriched).to_csv(OUTPUT_CSV, index=False, encoding="utf-8-sig")

    # JSON'a kaydet
    with open(OUTPUT_JSON, "w", encoding="utf-8") as f:
        json.dump(enriched, f, ensure_ascii=False, indent=2)

    print(f"\n✅ İşlem tamamlandı. {OUTPUT_CSV} ve {OUTPUT_JSON} dosyaları oluşturuldu.")

#  Ana çalıştırma
if __name__ == "__main__":
    enrich_all_products()
