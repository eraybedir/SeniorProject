import requests
import pandas as pd
import json
import math

FLASK_ENDPOINT = "http://localhost:5000/get-products"
DOTNET_ENDPOINT = "https://localhost:7121/api/product"

# 1. Flask'tan veri al
response = requests.get(FLASK_ENDPOINT)
data = response.json()

# 2. NaN, inf, -inf değerlerini null (None) yap
def sanitize(obj):
    for item in obj:
        for key, value in item.items():
            if isinstance(value, float):
                if math.isnan(value) or math.isinf(value):
                    item[key] = None
    return obj

data = sanitize(data)

# 3. .NET'e POST et
post_response = requests.post(DOTNET_ENDPOINT, json=data, verify=False)

print("Durum Kodu:", post_response.status_code)
print("Yanıt:", post_response.text)
