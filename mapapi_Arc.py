import arcade
import requests
import sys
import os

API_KEY_STATIC = 'f3a0fe3a-b07e-4840-a1da-06f18b2ddf13'
WINDOW_WIDTH = 650
WINDOW_HEIGHT = 500
WINDOW_TITLE = "MAP"
MAP_FILE = "map.png"


class GameView(arcade.Window):
    press_delta = 0.1

    def __init__(self, width, height, title, ll_spn=None, add_params=None):
        super().__init__(width, height, title)
        self.map_zoom = 10
        self.map_ll = ll_spn
        self.add_params = add_params
        self.background = None

    def setup(self):
        self.update_map()

    def on_draw(self):
        self.clear()

        if self.background:
            arcade.draw_texture_rect(
                self.background,
                arcade.LBWH(
                    (self.width - self.background.width) // 2,
                    (self.height - self.background.height) // 2,
                    self.background.width,
                    self.background.height
                ),
            )

    def on_key_press(self, key, modifiers):

        changed = False

        # масштаб
        if key == arcade.key.PAGEUP and self.map_zoom < 17:
            self.map_zoom += 1
            changed = True

        elif key == arcade.key.PAGEDOWN and self.map_zoom > 0:
            self.map_zoom -= 1
            changed = True

        # движение карты
        elif key == arcade.key.RIGHT:
            self.map_ll[0] += self.press_delta
            if self.map_ll[0] > 180:
                self.map_ll[0] -= 360
            changed = True

        elif key == arcade.key.LEFT:
            self.map_ll[0] -= self.press_delta
            if self.map_ll[0] < -180:
                self.map_ll[0] += 360
            changed = True

        elif key == arcade.key.UP:
            if self.map_ll[1] + self.press_delta < 90:
                self.map_ll[1] += self.press_delta
            changed = True

        elif key == arcade.key.DOWN:
            if self.map_ll[1] - self.press_delta > -90:
                self.map_ll[1] -= self.press_delta
            changed = True

        if changed:
            self.update_map()

    def update_map(self):

        ll = f"{self.map_ll[0]},{self.map_ll[1]}"

        map_request = (
            "https://static-maps.yandex.ru/v1?"
            f"apikey={API_KEY_STATIC}&ll={ll}&z={self.map_zoom}"
        )

        response = requests.get(map_request)

        if not response:
            print("Ошибка запроса:", response.status_code)
            sys.exit(1)

        with open(MAP_FILE, "wb") as file:
            file.write(response.content)

        self.background = arcade.load_texture(MAP_FILE)


def show_map(ll_spn=None, add_params=None):
    main(ll_spn, add_params)


def main(ll_spn=None, add_params=None):
    gameview = GameView(WINDOW_WIDTH, WINDOW_HEIGHT, WINDOW_TITLE, ll_spn, add_params)
    gameview.setup()
    arcade.run()
    # Удаляем за собой файл с изображением.
    os.remove(MAP_FILE)


if __name__ == '__main__':
    show_map("ll=37.530887,55.703118&spn=0.002,0.002")
