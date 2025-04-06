from selenium import webdriver
from selenium.webdriver.edge.service import Service
from selenium.webdriver.common.by import By
from webdriver_manager.microsoft import EdgeChromiumDriverManager
from bs4 import BeautifulSoup
import time
import pandas as pd
import os

# Kategoriler listesi
categories = [
    "et-tavuk-ve-balik",
    "icecek",
    "meyve-ve-sebze",
    "sut-ve-kahvaltilik",
    "gida",
]

base_url = "https://www.cimri.com/market/"

def scrape_cimri():
    all_products = []

    for category in categories:
        print(f"Şu an {category} kategorisini çekiyoruz...")
        service = Service(EdgeChromiumDriverManager().install())
        driver = webdriver.Edge(service=service)  # Tarayıcıyı başlat
        page = 1

        while True:
            url = f"{base_url}{category}?page={page}"
            driver.get(url)
            time.sleep(3)  # Sayfanın yüklenmesini bekle

            soup = BeautifulSoup(driver.page_source, "html.parser")

            # **1. Ürün isimlerini al**
            product_cards = soup.find_all("div", class_="ProductCard_productName__35zi5")
            product_names = [p.get_text(strip=True) for p in product_cards]

            # **2. Market bilgilerini al**
            market_tags = soup.find_all("div", class_="WrapperBox_wrapper__1_OBD")
            market_names = [m.find("img")["alt"] if m.find("img") else "Bilinmiyor" for m in market_tags]

            # **3. Fiyat bilgilerini al**
            # 3. Fiyat bilgilerini al - sadece ilk fiyat (TL) span'ını al
            price_list = []
            footer_cards = soup.find_all("div", class_="ProductCard_footer__Fc9OL")

            for footer in footer_cards:
                spans = footer.find_all("span", class_="ProductCard_price__10UHp")
                if spans:
                    price_text = spans[0].get_text(strip=True)
                    price_list.append(price_text)
                else:
                    price_list.append("Fiyat Bilinmiyor")


            # **4. Ürünleri fiyatlarla eşleştir**
            for i, name in enumerate(product_names):
                market = market_names[i] if i < len(market_names) else "Bilinmiyor"
                price = price_list[i] if i < len(price_list) else "Fiyat Bilinmiyor"

                product_info = {
                    "Kategori": category,
                    "Ürün İsmi": name,
                    "Market": market,
                    "Fiyat": price,
                }

                all_products.append(product_info)
                print(product_info)

            # **5. Sayfa kontrolü**
            next_page_btn = driver.find_elements(By.CSS_SELECTOR, "a[btnmode='next']")
            if next_page_btn:
                page += 1  # Eğer sonraki sayfa varsa artır
                print(f"{category} kategorisinde {page}. sayfaya geçiliyor...")
            else:
                print(f"{category} kategorisinde son sayfaya ulaşıldı. Diğer kategoriye geçiliyor...")
                break  # Son sayfa ise kategoriyi değiştir

        driver.quit()  # Tarayıcıyı kapat

    return all_products

# **Çalıştır ve sonucu yazdır**
products = scrape_cimri()
print(f"Toplam {len(products)} ürün çekildi!")

df = pd.DataFrame(products)
df.to_csv("products.csv",index=False,encoding="utf-8-sig")
csv_path = os.path.abspath("products.csv")
print(f"Veriler CSV dosyasına kaydedildi. {csv_path}")
