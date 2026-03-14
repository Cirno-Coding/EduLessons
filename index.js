const fruits = [
{ numb:1, name:"Apple", price:1.02 },
{ numb:2, name:"Pear", price:1.52 },
{ numb:3, name:"Banana", price:2.00 },
{ numb:4, name:"Mango", price:5.60 },
{ numb:5, name:"Orange", price:2.35 },
{ numb:6, name:"Lime", price:3.90 },
{ numb:7, name:"Apricot", price:4.10 },
{ numb:8, name:"Avocado", price:5.90 },
{ numb:9, name:"Papaya", price:7.00 },
{ numb:10, name:"Raspberry", price:4.60 },
{ numb:11, name:"Lemon", price:3.45 }
];


function renderProducts(arr) {
	const container = document.getElementById("productList");
	container.innerHTML = "";
	arr.forEach((fruit, index) => {
		const block = `
			<div class="col-auto">
				<div class="product-card text-center">
					<div class="product-number">#${fruit.numb}</div>
					<div class="product-name">
						${fruit.name.toUpperCase()}
					</div>
					<div class="product-price">
						${fruit.price} $
					</div>
				</div>
			</div>
		`;
		container.innerHTML += block;
	});
}


function mysort(arr, cmp) {
	for (let i = 0; i < arr.length - 1; i++) {
		for (let j = i + 1; j < arr.length; j++) {
			if (!cmp(arr[i], arr[j])) {
				let temp = arr[i];
				arr[i] = arr[j];
				arr[j] = temp;
			}
		}
	}
	return arr;
}


document.getElementById("sortBtn").addEventListener("click", () => {
	const type = document.getElementById("sortType").value;
	if (type === "name") {
		mysort(fruits, (a, b) => a.name < b.name);
	}
	if (type === "price") {
		mysort(fruits, (a, b) => a.price < b.price);
	}
	if (type === "numb") {
		mysort(fruits, (a, b) => a.numb < b.numb);
	}
	renderProducts(fruits);
});


renderProducts(fruits);