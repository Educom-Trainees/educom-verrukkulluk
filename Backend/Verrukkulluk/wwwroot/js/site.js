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
        fileLabel.innerHTML = 'Geen bestand gekozen';
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
    fileLabel.innerHTML = 'Geen bestand gekozen';
    img.style.display = 'none';
    removeButton.style.display = 'none';
}

//Tekstvak meebewegen met tekst:
function autoSize(element) {
    element.style.height = "auto";
    element.style.height = (element.scrollHeight) + "px";
}