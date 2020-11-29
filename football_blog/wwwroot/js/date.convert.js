const items = document.getElementsByClassName("utc-date");
for (let i = 0; i < items.length; i++) {
	let date = new Date(items[i].innerHTML);
	let(d.getMonth() + 1) + '.' + d.getDate() + '.' + d.getFullYear() + ' ' + d.getHours() + ':' + d.getMinutes() + ':' + d.getSeconds();
	items[i].innerHTML = newDate.getFullYear() + "-" + (newDate.getMonth()) + "-" + newDate.getDate() + " " + newDate.getHours() + ":" + newDate.getMinutes() + ":" + newDate.getSeconds();
}
