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
        print(f"📦 Şu an {category} kategorisini çekiyoruz...")
        service = Service(EdgeChromiumDriverManager().install())
        driver = webdriver.Edge(service=service)
        page = 1

        while True:
            url = f"{base_url}{category}?page={page}"
            driver.get(url)
            time.sleep(3)

            soup = BeautifulSoup(driver.page_source, "html.parser")

            # ✅ Ürün adları
            product_cards = soup.find_all("div", class_="ProductCard_productName__35zi5")
            product_names = [p.get_text(strip=True) for p in product_cards]

            # ✅ Market isimleri
            market_tags = soup.find_all("div", class_="WrapperBox_wrapper__1_OBD")
            market_names = [m.find("img")["alt"] if m.find("img") else "Bilinmiyor" for m in market_tags]

            # ✅ Fiyatlar (ilk fiyat span'ı)
            price_list = []
            footer_cards = soup.find_all("div", class_="ProductCard_footer__Fc9OL")
            for footer in footer_cards:
                spans = footer.find_all("span", class_="ProductCard_price__10UHp")
                if spans:
                    price_text = spans[0].get_text(strip=True)
                    price_list.append(price_text)
                else:
                    price_list.append("Fiyat Bilinmiyor")

            # ✅ Görsel URL'leri
            image_containers = soup.find_all("div", class_="ProductCard_imageContainer__ASSCc")
            image_urls = [
                div.find("img")["src"] if div.find("img") and div.find("img").has_attr("src") else "Yok"
                for div in image_containers
            ]

            # ✅ Ürün bilgilerini birleştir
            for i, name in enumerate(product_names):
                product_info = {
                    "Kategori": category,
                    "Ürün İsmi": name,
                    "Market": market_names[i] if i < len(market_names) else "Bilinmiyor",
                    "Fiyat": price_list[i] if i < len(price_list) else "Fiyat Bilinmiyor",
                    "ImageUrl": image_urls[i] if i < len(image_urls) else "Yok"
                }

                all_products.append(product_info)
                print(product_info)

            # ✅ Sayfa kontrolü
            next_page_btn = driver.find_elements(By.CSS_SELECTOR, "a[btnmode='next']")
            if next_page_btn:
                page += 1
                print(f"📄 {category} kategorisinde {page}. sayfaya geçiliyor...")
            else:
                print(f"✅ {category} kategorisi bitti.")
                break

        driver.quit()

    return all_products

# ✅ Çalıştır ve CSV'ye kaydet
products = scrape_cimri()
print(f"✅ Toplam {len(products)} ürün çekildi!")

df = pd.DataFrame(products)
df.to_csv("products_new.csv", index=False, encoding="utf-8-sig")
csv_path = os.path.abspath("products_new.csv")
print(f"📁 Veriler CSV dosyasına kaydedildi: {csv_path}")
