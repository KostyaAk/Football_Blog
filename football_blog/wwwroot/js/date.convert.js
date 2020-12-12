const items = document.getElementsByClassName("utc-date");
for (let i = 0; i < items.length; i++) {
	let d = new Date(items[i].innerHTML);
	let newDate = new Date(Date.UTC(d.getUTCFullYear(), d.getUTCMonth(), d.getUTCDate(),  d.getHours() , d.getMinutes() ,d.getSeconds()));
	items[i].innerHTML = newDate.getFullYear() + "-" + (newDate.getMonth() + 1) + "-" + newDate.getDate() + " " + newDate.getHours() + ":" + newDate.getMinutes() + ":" + newDate.getSeconds();
}
