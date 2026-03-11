const usersContainer = document.getElementById("users");
const userInfo = document.getElementById("userInfo");
const postsContainer = document.getElementById("posts");
const showPostsBtn = document.getElementById("showPostsBtn");

let currentUserId = null;

loadUsers();

function loadUsers(){
	const xhr = new XMLHttpRequest();
	xhr.open("GET","https://jsonplaceholder.typicode.com/users");
	xhr.onload = function(){
		const users = JSON.parse(xhr.responseText);
		users.forEach(user => {
			const col = document.createElement("div");
			col.className="col-md-4 mb-4";
			col.innerHTML = `
			<div class="user-card">
			<h5>${user.name}</h5>
			<p>${user.email}</p>
			</div>
			`;
			col.onclick = () => loadUser(user.id);
			usersContainer.appendChild(col);
		});
	};
	xhr.send();
}

function loadUser(id){
	currentUserId = id;
	const xhr = new XMLHttpRequest();
	xhr.open("GET","https://jsonplaceholder.typicode.com/users/"+id);
	xhr.onload = function(){
		const user = JSON.parse(xhr.responseText);
		userInfo.innerHTML = `
		<h3>User Info</h3>
		<table class="table table-bordered">
			<tr>
				<th>Name</th>
				<td>${user.name}</td>
			</tr>
			<tr>
				<th>Username</th>
				<td>${user.username}</td>
			</tr>
			<tr>
				<th>Email</th>
				<td>${user.email}</td>
			</tr>
			<tr>
				<th>Phone</th>
				<td>${user.phone}</td>
			</tr>
			<tr>
				<th>Website</th>
				<td>${user.website}</td>
			</tr>
			<tr>
				<th>Company</th>
				<td>${user.company.name}</td>
			</tr>
		</table>
		`;
		showPostsBtn.classList.remove("d-none");
		postsContainer.innerHTML="";
	};
	xhr.send();
}

showPostsBtn.addEventListener("click",loadPosts);
function loadPosts(){
	const xhr = new XMLHttpRequest();
	xhr.open("GET","https://jsonplaceholder.typicode.com/posts?userId="+currentUserId);
	xhr.onload = function(){
		const posts = JSON.parse(xhr.responseText);
		postsContainer.innerHTML="<h3 class='mb-4'>User's Posts</h3>";
		posts.forEach(post => {
			const col = document.createElement("div");
			col.className="col-md-6";
			col.innerHTML = `
			<div class="post">
			<h5>${post.title}</h5>
			<p>${post.body}</p>
			</div>
			`;
			postsContainer.appendChild(col);
		});
	};
	xhr.send();
}