from mapapi_Arc import show_map


def main():
    map_zoom = 10
    map_ll = [37.977751, 55.757718]

    # строка координат
    ll = f"{map_ll[0]},{map_ll[1]}"

    # параметры карты
    params = f"ll={ll}&z={map_zoom}"

    # запуск окна Arcade с картой
    show_map(map_ll, add_params=f"z={map_zoom}")


if __name__ == "__main__":
    main()
