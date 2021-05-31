const itemsMenu = document.querySelector('.items');
const closeMenu = document.querySelector('.closeMenu');
const openMenu = document.querySelector('.openMenu');




openMenu.addEventListener('click',show);
closeMenu.addEventListener('click',close);

function show(){
    itemsMenu.style.display = 'flex';
    itemsMenu.style.top = '0';
}
function close(){
    itemsMenu.style.top = '-100%';
}