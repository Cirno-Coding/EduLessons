import pprint

import requests

API_KEY = ""


def find_value(data, target, path="data"):
    if isinstance(data, dict):
        for key, value in data.items():
            new_path = f"{path}['{key}']"
            if value == target:
                return new_path
            result = find_value(value, target, new_path)
            if result:
                return result

    elif isinstance(data, list):
        for i, value in enumerate(data):
            new_path = f"{path}[{i}]"
            if value == target:
                return new_path
            result = find_value(value, target, new_path)
            if result:
                return result

    return None


# Шаг 1. Ищем код станции (теоретически это делается один раз)
# Но мы же учимся, давайте глянем, как найти код Москвы (c213) в этой куче данных
# ВНИМАНИЕ: этот запрос тяжёлый, лучше сохранить результат в файлик

def get_station_code():
    url = f'https://api.rasp.yandex.net/v3.0/stations_list/?apikey={API_KEY}&format=json&lang=ru_RU'
    print("⏳ Скачиваю список всех станций мира... (это может занять время)")

    response = requests.get(url)
    data = response.json()
    path = find_value(data, "Москва")
    print(path)

    # Путь ниндзя к коду Элисты через дебри JSON:
    # Страны -> Россия (0) -> Регионы -> Москва (0) -> Населённые пункты -> Москва (0)
    # P. S. Индексы могут меняться, в реальном коде так жёстко их не зашивают, но для примера сойдёт
    try:
        moscow_code = data['countries'][0]['regions'][0]['settlements'][0]['codes']['yandex_code']
        print(f"🎉 Эврика! Код Москвы: {moscow_code}")
    except (KeyError, IndexError):
        print("Что-то пошло не так, возможно, индексы сдвинулись. Жизнь — боль.")
    try:
        volgagrad_code = data['countries'][0]['regions'][9]['settlements'][0]['codes']['yandex_code']
        print(f"🎉 Эврика! Код Волгограда: {volgagrad_code}")
    except (KeyError, IndexError):
        print("Что-то пошло не так, возможно, индексы сдвинулись. Жизнь — боль.")


# get_station_code()  # Раскомментируйте, если смелые


url = f"https://api.rasp.yandex-net.ru/v3.0/search/"
from_, to_ = input().split()
date = input()
params = {
    "apikey": API_KEY,
    "format": "json",
    "from": from_,
    "to": to_,
    "lang": "ru_RU",
    "date": date
}
response = requests.get(url, params=params)
print(response.url)
data = response.json()["segments"]
trains = set()
for item in data:
    if item["thread"]["transport_type"] == "train":
        trains.add(item["thread"]["title"])
print(*sorted(trains), sep='\n')

# Пример:
# c213 c38
