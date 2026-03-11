const input = document.getElementById("jsonInput");
const result = document.getElementById("result");
const button = document.getElementById("formatBtn");

button.addEventListener("click", () => {

    try {

        // преобразование строки в объект
        const jsonObject = JSON.parse(input.value);

        // форматирование с отступом 4 пробела
        const formattedJSON = JSON.stringify(jsonObject, null, 4);

        result.textContent = formattedJSON;

    } catch (error) {

        result.textContent = "Ошибка: введённые данные не являются корректным JSON.";

    }

});