import requests

access_key = "8a51d65c-ba34-479e-8a5d-cc347c8e5acb"

headers = {
    "X-Yandex-Weather-Key": access_key
}
lat, lon, q = map(float, input().split())

query1 = f"""
    {{
        weatherByPoint(request: {{ lat: {lat}, lon: {lon} }}) {{
            now {{
                humidity
                pressure
                temperature
                windSpeed
                windDirection
            }}
        }}
    }}"""

response = requests.post('https://api.weather.yandex.ru/graphql/query',
                         headers=headers, json={'query': query1})
t1 = response.json()['data']['weatherByPoint']['now']

query2 = f"""
    {{
        weatherByPoint(request: {{ lat: {lat}, lon: {lon + q} }}) {{
            now {{
                humidity
                pressure
                temperature
                windSpeed
                windDirection
            }}
        }}
    }}"""

response = requests.post('https://api.weather.yandex.ru/graphql/query',
                         headers=headers, json={'query': query2})
t2 = response.json()['data']['weatherByPoint']['now']

params = 'humidity\tpressure\ttemperature\twindSpeed\twindDirection'.split()
for k in params:
    print(f'{k}\t{t1[k]}\t{t2[k]}')

# Пример:
# 44.2558, 46.3078 1.5
