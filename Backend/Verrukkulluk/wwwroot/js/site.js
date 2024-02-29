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
    var ratingValue = 0;
    var rated = false;
    var comment = "";

    $('#recipeRating i').hover(function () {
        if (!rated) {
            var index = $(this).index();
            $('#recipeRating i').removeClass('bi-star-fill').addClass('bi-star');
            $('#recipeRating i:lt(' + (index + 1) + ')').removeClass('bi-star').addClass('bi-star-fill');
        }
    }, function () {
        if (!rated) {
            $('#recipeRating i').removeClass('bi-star-fill').addClass('bi-star');
            $('#recipeRating i:lt(' + ratingValue + ')').removeClass('bi-star').addClass('bi-star-fill');
        }
    });

    $('#recipeRating i').on('click', function () {
        if (!rated) {
            ratingValue = $(this).data('value');
        }
    });

    $('#confirmRating').on('click', function () {
        var comment = $('#comment').val();
        if (ratingValue < 1) {
            $('#ratingMessage').text('Geef een beoordeling tussen de 1 en 5');
            $('#confirmRating').prop('disabled', false);
        } else {
            rateRecipe(ratingValue, comment);
            rated = true;
            $('#confirmRating').remove();
        }
    });

    fetchUserRating();

    function fetchUserRating() {
        $.ajax({
            type: 'GET',
            url: '/Verrukkulluk/GetUserRating',
            data: { recipeId: $('#recipeId').val() },
            success: function (response) {
                if (response.rating > 0) {
                    ratingValue = response.rating;
                    displayRating();
                    updateStars(response.rating);
                    $('#confirmRating').text('Verander uw beoordeling');
                }
                if (response.rating < 0) {
                    ratingValue = -response.rating;
                    displayRating();
                    updateStars(-response.rating);
                    disableStarRating();
                }
                if (response.comment) {
                    $('#comment').val(response.comment);
                }
            },
            error: function () {
                console.error('Error occurred while fetching user rating.');
            }
        });
    }

    function displayRating() {
        $('#userRating').text('Uw beoordeling:');
    }

    function updateStars(rating) {
        $('#recipeRating i').each(function (index) {
            if (index < rating) {
                $(this).removeClass('bi-star').addClass('bi-star-fill');
            }
        });
    }

    function disableStarRating() {
        $('#recipeRating i').off('click');
        $('#recipeRating i').off('mouseenter mouseleave');
        $('#confirmRating').remove();
    }
});

//Adding ingredient in CreateRecipe
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
        '<input type="hidden" name="AddedIngredients['+ingredientCount+'].Name" value="' + product.name + '">' +
        '<input type="hidden" name="AddedIngredients['+ingredientCount+'].ProductId" value="'+product.id+'">'+
        '<div class="col-1">'+product.ingredientType+'</div>'+
        '<div class="darker-red bi bi-trash product-trash col-1" onclick="removeIngredient(this)"></div>'+
        '</div>'
    $("#ingredients").append(template);
    $("#AddedIngredients_"+ingredientCount).focus();
    ingredientCount++;
    $("#newProduct")[0].value = "";

    product.productAllergies.forEach(updateAllergyInfo);
}

//Adding allergy in CreateRecipe
function updateAllergyInfo(productAllergy) {
    removeNoAllergies();
    console.log(productAllergy);
    const allergy = productAllergy.allergy;
    const existingAllergy = $(".allergy[id="+allergy.id+"]")
    console.log(allergy);
    console.log(existingAllergy);
    if (existingAllergy.length>0){
        existingAllergy[0].dataset.count++; 
        return
    } 
    const template = '<img class="allergy" id="'+allergy.id+'" src ="/Image/GetImage/'+allergy.imgObjId+'" alt="'+allergy.name+'" data-count="1" style="width:70px; height:auto">'
    $("#allergies").append(template);
}
//Remove 'No allergies'
function removeNoAllergies() {
    $('.noAllergy').remove();
}
function showNoAllergies() {
    const template = '<img class="noAllergy" id="noAllergy" src ="/images/allergenen/geen.png" style="width:70px; height:auto"></img>'
    $("#allergies").append(template);
}

//Remove ingredient with trash icon
function removeIngredient(element) {
    const productId = element.parentElement.childNodes[4].value;
    collectProduct(productId);
    $(element).closest('.row').remove(); 
}
function collectProduct(productId) {
    $.get("../api/products/" + productId, function(data, status){
        if (status === "success") {
            updateAllergies(data)
        }
    });
}
function updateAllergies(product){
    product.productAllergies.forEach(decreaseAllergyCount);
}
function decreaseAllergyCount(productAllergy) {
    const allergy = productAllergy.allergy;
    const existingAllergy = $(".allergy[id="+allergy.id+"]")
    existingAllergy[0].dataset.count--; 
    if (existingAllergy[0].dataset.count == 0) {
        existingAllergy.remove();
        if($('.allergy').length == 0) {
            showNoAllergies();
        }
    }
    return
}

//Adding photo in CreateRecipe
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
//Remove photo in CreateRecipe
function removeFile() {
    var input = document.getElementById('DishPhoto');
    var fileLabel = document.getElementById('fileLabel');
    var img = document.getElementById('preview');
    var removeButton = document.getElementById('removeButton');
    var deleteExistingImage = document.getElementById('DeleteImage');

    input.value = '';
    fileLabel.innerHTML = 'Geen afbeelding geselecteerd...';
    img.style.display = 'none';
    removeButton.style.display = 'none';
    deleteExistingImage.value = 'true';
}

//Growing textarea with content in CreateRecipe
function autoSize(element) {
    element.style.height = "auto";
    element.style.height = (element.scrollHeight) + "px";
}

//Preview ProfilePicture in Register en Manage
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

//Remove ProfilePicture in Register en Manage
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

function handleChevronScroll(tabContent) {
    if (tabContent.offsetHeight > 0) {
        handleScrollChevronDown();
        handleScrollChevronUp();
    }
}

function tabShownHandler(event) {
    var tabContentId = event.target.getAttribute('href');
    var tabContent = document.querySelector(tabContentId);
    if (tabContent) {
        handleChevronScroll(tabContent);
    }
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

    // Attach event listeners to tab shown events
    document.querySelectorAll('.nav-link-recipe').forEach(function (link) {
        link.addEventListener('shown.bs.tab', tabShownHandler);
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


function scrollToEvent(event, eventNumber) {
    event.preventDefault();
    var eventContainer = document.getElementById('event' + eventNumber);
    var agendaContainer = document.getElementById('agendaScroll');
    var scrollToY = eventContainer.offsetTop - agendaContainer.offsetTop;
    agendaContainer.scrollTop = scrollToY;
}

//Instructions in Create Recipe
function addInstructionStep(inputElement) {
    if (inputElement.getAttribute('added-textarea') !== 'true' && inputElement.value.trim() !== '') {
        const nextInstructionStepDiv = document.createElement('div');
        nextInstructionStepDiv.className = 'form-group form-floating instruction';

        const nextInstructionStep = document.createElement('textarea');
        nextInstructionStep.className = 'form-control instructionSteps';
        nextInstructionStep.name = `Instructions[${document.getElementsByClassName('instruction').length}]`;
        nextInstructionStep.oninput = function () {
            autoSize(this);
        };
        nextInstructionStep.onkeydown = function () {
            addInstructionStep(this);
        };

        const nextInstructionLabel = document.createElement('label');
        nextInstructionLabel.className = 'control-label';
        nextInstructionLabel.textContent = `Stap ${document.getElementsByClassName('instruction').length + 1}`;

        nextInstructionStepDiv.appendChild(nextInstructionStep);
        nextInstructionStepDiv.appendChild(nextInstructionLabel);
        inputElement.parentNode.appendChild(nextInstructionStepDiv);

        inputElement.setAttribute('added-textarea', 'true');
    }
}

function rateRecipe(ratingValue, comment) {
    var comment = $("#comment").val();

    $.ajax({
        type: 'POST',
        url: '/Verrukkulluk/RateRecipe',
        data: { recipeId: $('#recipeId').val(), ratingValue: ratingValue, comment: comment },
        success: function (rating) {
            if (rating !== null) {
                updateAverageRating($('#recipeId').val());
                addOrUpdateCommentView(rating);
                $('#ratingMessage').text('Bedankt voor uw feedback!');
            } else {
                $('#ratingMessage').text('Er ging iets mis, probeer opnieuw.');
            }
        },
        error: function () {
            $('#ratingMessage').text('An error occurred while submitting rating.');
        }
    });
}
function addOrUpdateCommentView(rating) {
    var ratingId = rating.id;
    var existingRating = document.querySelector('[data-rating-id="' + ratingId + '"]');
    if (existingRating)
    {
        console.log("Div is gevonden")
        //updaten van de rating
    } else {
        console.log("Div is niet gevonden")
        //toevoegen van de rating
    }
}

function updateAverageRating(recipeId) {
    $.ajax({
        type: 'POST',
        url: '/Verrukkulluk/UpdateAverageRating',
        data: { recipeId: recipeId },
        success: function (response) {
            if (response.success) {
                console.log('Average rating updated successfully.');
            } else {
                console.error('Failed to update average rating.');
            }
        },
        error: function () {
            console.error('An error occurred while updating the average rating.');
        }
    });
}

function validateForm() {
    var fileInput = document.getElementById('Input.ProfilePicture');
    var errorMessage = document.getElementById('profilePictureError');
    if (fileInput.files.length === 0) {
        errorMessage.textContent = 'Selecteer uw profielfoto';
        return false;
    } else {
        errorMessage.textContent = '';
    }
    return true;
}

function showConfirmationRecipe(recipeId, recipeName) {
    updateConfirmationMessage("Weet u zeker dat u het recept '" + recipeName + "' wilt verwijderen?");
    
    $('#confirmationScreen').modal('show');
    
    $('#confirmButton').on('click', function () {
        $('#removeRecipeForm_' + recipeId).submit();
    });
}

function showConfirmationUser(userId, firstName) {
    updateConfirmationMessage("Weet u zeker dat u de gebruiker '" + firstName + "' wilt verwijderen?");

    $('#confirmationScreen').modal('show');

    $('#confirmButton').on('click', function () {
        $('#removeUserForm_' + userId).submit();
    });
}

function updateConfirmationMessage(message) {
    var confirmationMessageElement = document.getElementById('confirmationMessage');
    if (confirmationMessageElement) {
        confirmationMessageElement.innerText = message;
    }
}

