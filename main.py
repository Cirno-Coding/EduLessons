from mapapi_Arc import show_map


def main():
    map_zoom = 10
    map_ll = [44.2558, 46.3078]

    # запуск окна Arcade с картой
    show_map(map_ll, add_params=f"z={map_zoom}")


if __name__ == "__main__":
    main()
