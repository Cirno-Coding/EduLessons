import requests
import pprint


# Ввод данных
folder = input().strip()
# Публикуем и получаем ссылку
headers = {
    'Authorization': ''
}
requests.put("https://cloud-api.yandex.net/v1/disk/resources/publish",
             headers=headers, params={"path": folder})

# Получаем ссылку
response = requests.get("https://cloud-api.yandex.net/v1/disk/resources",
                        headers=headers,
                        params={"path": folder, "fields": "public_url"})

print(response.json().get('public_url', response.text))
