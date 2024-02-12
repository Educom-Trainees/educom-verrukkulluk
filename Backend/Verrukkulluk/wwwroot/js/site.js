// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function(){

    // jQuery methods go here...
    $("#addProduct").click(addProduct)

    $('#deleteRecipeModal').on('show.bs.modal', function (event) {
        var button = $(event.relatedTarget);
        var recipeId = button.data('recipe-id');
        var recipeTitle = button.data('recipe-title');
        var deleteRecipeTitle = $('#deleteRecipeTitle');

        deleteRecipeTitle.text('Weet u zeker dat u het recept "' + recipeTitle + '" wilt verwijderen?');

        //Recipe is removed when confirmation is given
        $('#confirmDeleteBtn').on('click', function () {
            window.location.href = '/Verrukkulluk/ReceptVerwijderen/' + recipeId;
        });
    });
    $('.quantity-input').on('input', function () {
        // Perform actions when the quantity changes
        // You can retrieve the new quantity and item details using JavaScript
        var newQuantity = $(this).val();
        // Perform further actions, e.g., update the total price
    });
    $('#addToShoppingListForm').submit(function (e) {
        e.preventDefault();
        var formData = $(this).serialize();
        
        $('#addToShoppingListButton').prop('disabled', true);

        $.ajax({
            type: 'POST',
            url: $(this).attr('action'),
            data: formData,
            success: function (data) {
                if (data.success) {
                    displaySuccessMessage("Het recept is toegevoegd aan uw boodschappenlijst!");
                } else {
                    console.error(data.message);
                }
            },
            error: function (xhr, status, error) {
                console.error('Error adding recipe to shopping list:', error);
            },
            complete: function () {
            }
        });
    });
});

function addProduct(e)
{
    e.preventDefault();
    const option = findProductToAdd()
    const error = $("#newProductErr");
    if (option === undefined) {
        // set error
        error.text("product niet gevonden");
        return;
    }
    error.text("");
    const productId = option.attributes["data-product-id"].value

    $.get("../api/products/" + productId, function(data, status){
        if (status === "success") {
            addIngredient(data)
        }
      });
}

function findProductToAdd() 
{
    const input = $("#newProduct")[0].value
    const productsList = $("#productsList").children()
    console.log(productsList);
    const entries = productsList.filter(optionElement => productsList[optionElement].value == input)
    console.log(entries);
    return entries[0]
}

function addIngredient(product) {
    console.log(product);
    const template = '<div class="row">'+
        '<label class="col-4">'+product.name+'</label>'+
        '<input type="number" class="col-2 formField" name="AddedIngredients['+ingredientCount+'].Amount" id="AddedIngredients_'+ingredientCount+'" value="">'+
        '<input type="hidden" name="AddedIngredients['+ingredientCount+'].Id" value="0">'+
        '<input type="hidden" name="AddedIngredients['+ingredientCount+'].ProductId" value="'+product.id+'">'+
        '<div class="col-2">'+product.ingredientType+'</div>'+
    '</div>'
    $("#ingredients").append(template);
    $("#AddedIngredients_"+ingredientCount).focus();
    ingredientCount++;
    $("#newProduct")[0].value = "";
}

//Foto toevoegen aan Recept Maken:
function handleFileSelect(input) {
    var files = input.files;
    var fileLabel = document.getElementById('fileLabel');
    var img = document.getElementById('preview');
    var removeButton = document.getElementById('removeButton');
    if (files.length > 0) {
        fileLabel.innerHTML = files[0].name;
        removeButton.style.display = 'inline-block';

        var reader = new FileReader();
        reader.onload = function (e) {
            img.src = e.target.result;
            img.style.display = 'block';
        };
        reader.readAsDataURL(files[0]);
    } else {
        fileLabel.innerHTML = 'Geen afbeelding geselcteerd...';
        img.style.display = 'none';
        removeButton.style.display = 'none';
    }
}
//Foto verwijderen bij Recept Maken:
function removeFile() {
    var input = document.getElementById('DishPhoto');
    var fileLabel = document.getElementById('fileLabel');
    var img = document.getElementById('preview');
    var removeButton = document.getElementById('removeButton');

    input.value = '';
    fileLabel.innerHTML = 'Geen afbeelding geselecteerd...';
    img.style.display = 'none';
    removeButton.style.display = 'none';
}
//Foto profiel en preview
function handleFileSelectProfilePicture(input) {
    var files = input.files;
    var fileLabel = document.getElementById('fileLabel');
    var img = document.getElementById('preview');
    var removeButton = document.getElementById('removeButton');
    if (files.length > 0) {
        fileLabel.innerHTML = files[0].name;
        removeButton.style.display = 'inline-block';

        var reader = new FileReader();
        reader.onload = function (e) {
            img.src = e.target.result;
            img.style.display = 'block';
        };
        reader.readAsDataURL(files[0]);
    } else {
        fileLabel.innerHTML = 'Selecteer uw profielfoto...';
        img.style.display = 'none';
        removeButton.style.display = 'none';
    }
}

//Foto verwijderen profiel
function removeFileProfilePicture() {
    var input = document.getElementById('Input.ProfilePicture');
    var fileLabel = document.getElementById('fileLabel');
    var img = document.getElementById('preview');
    var removeButton = document.getElementById('removeButton');

    input.value = '';
    fileLabel.innerHTML = 'Selecteer uw profielfoto...';
    img.style.display = 'none';
    removeButton.style.display = 'none';
}

//Tekstvak meebewegen met tekst:
function autoSize(element) {
    element.style.height = "auto";
    element.style.height = (element.scrollHeight) + "px";
}

// Remove all shopping items from cart
function removeAllItems() {
    fetch('/RemoveAllShopItems')
        .then(response => response.json())
        .then(data => {
            location.reload();
        });
}

// Handle checking/unchecking items
function handleCheckmarkClick() {
    var row = this.closest('.container.text-start.shopping-list-text');
    row.style.textDecoration = row.style.textDecoration === 'line-through' ? 'none' : 'line-through';
}

// Remove a shopping item from cart
function removeItem() {
    var trashIcon = event.target;
    
    var row = trashIcon.closest('.container.text-start.shopping-list-text');
    
    var itemName = row.querySelector('.darker-green').innerText.trim();
    
    console.log('Removing item:', itemName);
    
    fetch(`/RemoveShopItemByName?itemName=${itemName}`, { method: 'POST' })
        .then(response => response.json())
        .then(data => {
            if (data.success)
            {
                location.reload();
            }
            else
            {
                console.error(data.message);
            }
        });
}

// Show/hide chevron-down shopping list
function handleScrollChevronDown() {
    document.querySelectorAll('.scrolling-content').forEach(function(scrollingContent) {
        var chevronDown = scrollingContent.nextElementSibling.querySelector('.bi-chevron-down');
        var isAtBottom = scrollingContent.scrollHeight - scrollingContent.clientHeight <= scrollingContent.scrollTop + 1;
        chevronDown.style.visibility = isAtBottom ? 'hidden' : 'visible';
    });
}

// Show/hide chevron-up shopping list
function handleScrollChevronUp() {
    document.querySelectorAll('.scrolling-content').forEach(function(scrollingContent) {
        var chevronUp = scrollingContent.previousElementSibling.querySelector('.bi-chevron-up');
        var isAtTop = scrollingContent.scrollTop <= 0;
        chevronUp.style.visibility = isAtTop ? 'hidden' : 'visible';
    });
}

// Event listeners
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.bi-check').forEach(function (checkmark) {
        checkmark.addEventListener('click', handleCheckmarkClick);
    });

    document.querySelectorAll('.product-trash').forEach(function (trashIcon) {
        trashIcon.addEventListener('click', removeItem);
    });

    document.querySelectorAll('.scrolling-content').forEach(function (scrollingContent) {
        scrollingContent.addEventListener('scroll', handleScrollChevronDown);
        handleScrollChevronDown();
        
        scrollingContent.addEventListener('scroll', handleScrollChevronUp);
        handleScrollChevronUp();
    });
});


// Display success message
function displaySuccessMessage(message) {
    var successMessage = document.getElementById('successMessage');
    successMessage.innerText = message;
    successMessage.style.display = 'block';
}

function setHeightAgenda() {
    if ($('.agenda-scroll').css('overflow-y') === 'auto')
    {
        var flexBoxHeight = $('.d-inline-flex').height();
        $('.agenda-scroll').css('max-height', flexBoxHeight);
    }
    else
    {
        $('.agenda-scroll').css('max-height', '');
    }
}
$(document).ready(function () {
    setHeightAgenda();

    $(window).resize(function () {
        setHeightAgenda();
    });
});

window.addEventListener('DOMContentLoaded', function () {
    var chevronDown = document.querySelector('.bi-chevron-down');
    var agendaScroll = document.getElementById('agendaScroll');
    
    function toggleChevronVisibility() {
        if (window.innerWidth <= 767) {
            chevronDown.classList.add('visible-on-small-screen');
        } else {
            chevronDown.classList.remove('visible-on-small-screen');
        }
    }
    toggleChevronVisibility();
    window.addEventListener('resize', toggleChevronVisibility);
});

function scrollToEvent(event, eventNumber) {
    event.preventDefault();
    var eventContainer = document.getElementById('event' + eventNumber);
    var agendaContainer = document.getElementById('agendaScroll');
    var scrollToY = eventContainer.offsetTop - agendaContainer.offsetTop;
    agendaContainer.scrollTop = scrollToY;
}




