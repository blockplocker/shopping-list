using Microsoft.AspNetCore.Authorization;
using ShoppingList.Application.Interfaces;
using ShoppingList.Application.Services;
using ShoppingList.Domain.Models;
using Xunit;

namespace ShoppingList.Tests;

/// <summary>
/// Unit tests for ShoppingListService.
///
/// IMPORTANT: Write your tests here using Test Driven Development (TDD)
///
/// TDD Workflow:
/// 1. Write a test for a specific behavior (RED - test fails)
/// 2. Implement minimal code to make the test pass (GREEN - test passes)
/// 3. Refactor the code if needed (REFACTOR - improve without changing behavior)
/// 4. Repeat for the next behavior
///
/// Test Examples:
/// - See ShoppingItemTests.cs for examples of well-structured unit tests
/// - Follow the Arrange-Act-Assert pattern
/// - Use descriptive test names: Method_Scenario_ExpectedBehavior
///
/// What to Test:
/// - Happy path scenarios (normal, expected usage)
/// - Input validation (null/empty IDs, invalid parameters)
/// - Edge cases (empty array, array expansion, last item, etc.)
/// - Array management (shifting after delete, compacting, reordering)
/// - Search functionality (case-insensitive, matching in name/notes)
///
/// Recommended Test Categories:
///
/// GetAll() tests:
/// - GetAll_WhenEmpty_ShouldReturnEmptyList
/// - GetAll_WithItems_ShouldReturnAllItems
/// - GetAll_ShouldNotReturnMoreThanActualItemCount
///
/// GetById() tests:
/// - GetById_WithValidId_ShouldReturnItem
/// - GetById_WithInvalidId_ShouldReturnNull
/// - GetById_WithNullId_ShouldReturnNull
/// - GetById_WithEmptyId_ShouldReturnNull
///
/// Add() tests:
/// - Add_WithValidInput_ShouldReturnItem
/// - Add_ShouldGenerateUniqueId
/// - Add_ShouldIncrementItemCount
/// - Add_WhenArrayFull_ShouldExpandArray
/// - Add_AfterArrayExpansion_ShouldContinueWorking
/// - Add_ShouldSetIsPurchasedToFalse
///
/// Update() tests:
/// - Update_WithValidId_ShouldUpdateAndReturnItem
/// - Update_WithInvalidId_ShouldReturnNull
/// - Update_ShouldNotChangeId
/// - Update_ShouldNotChangeIsPurchased
///
/// Delete() tests:
/// - Delete_WithValidId_ShouldReturnTrue
/// - Delete_WithInvalidId_ShouldReturnFalse
/// - Delete_ShouldRemoveItemFromList
/// - Delete_ShouldShiftRemainingItems
/// - Delete_ShouldDecrementItemCount
/// - Delete_LastItem_ShouldWork
/// - Delete_FirstItem_ShouldWork
/// - Delete_MiddleItem_ShouldWork
///
/// Search() tests:
/// - Search_WithEmptyQuery_ShouldReturnAllItems
/// - Search_WithNullQuery_ShouldReturnAllItems
/// - Search_MatchingName_ShouldReturnItem
/// - Search_MatchingNotes_ShouldReturnItem
/// - Search_ShouldBeCaseInsensitive
/// - Search_WithNoMatches_ShouldReturnEmpty
/// - Search_ShouldFindPartialMatches
///
/// ClearPurchased() tests:
/// - ClearPurchased_WithNoPurchasedItems_ShouldReturnZero
/// - ClearPurchased_ShouldRemoveOnlyPurchasedItems
/// - ClearPurchased_ShouldReturnCorrectCount
/// - ClearPurchased_ShouldShiftRemainingItems
///
/// TogglePurchased() tests:
/// - TogglePurchased_WithValidId_ShouldReturnTrue
/// - TogglePurchased_WithInvalidId_ShouldReturnFalse
/// - TogglePurchased_ShouldToggleFromFalseToTrue
/// - TogglePurchased_ShouldToggleFromTrueToFalse
///
/// Reorder() tests:
/// - Reorder_WithValidOrder_ShouldReturnTrue
/// - Reorder_WithInvalidId_ShouldReturnFalse
/// - Reorder_WithMissingIds_ShouldReturnFalse
/// - Reorder_WithDuplicateIds_ShouldReturnFalse
/// - Reorder_ShouldChangeItemOrder
/// - Reorder_WithEmptyList_ShouldReturnFalse
/// </summary>
public class ShoppingListServiceTests
{
    // TODO: Write your tests here following the TDD workflow

    // Example test structure:
    // [Fact]
    // public void Add_WithValidInput_ShouldReturnItem()
    // {
    //     // Arrange
    //     var service = new ShoppingListService();
    //
    //     // Act
    //     var item = service.Add("Milk", 2, "Lactose-free");
    //
    //     // Assert
    //     Assert.NotNull(item);
    //     Assert.Equal("Milk", item!.Name);
    //     Assert.Equal(2, item.Quantity);
    // }

    [Theory]
    [InlineData("Milk", 2, "Lactose-free")]
    [InlineData("Bread", 4, "grain")]
    [InlineData("jucie", 5, null )]
    public void Add_ShouldreturnValidItem(string name, int quantity, string? notes)
    {
        // Arrange
        var service = new ShoppingListService();
        var expected = new ShoppingItem()
        {
            Name = name,
            Quantity = quantity,
            Notes = notes
        };

        // Act
        var actual = service.Add(name, quantity, notes);        

        // Assert
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(actual.Quantity, expected.Quantity);
        Assert.Equal(actual.Notes, expected.Notes);
    }
    
    [Theory]
    [InlineData("milk", 2, "Lactose-free")]
    public void Add_ShouldAddValidItemToItemsArray(string name, int quantity, string? notes)
    {
        // Arrange
        var service = new ShoppingListService();
        var expectedLenght = 1;
        
        // Act
        var item = service.Add(name, quantity, notes);
        var actualLenght = service._TEST_items.Length;
        var actualItem = service._TEST_items[0];

        // Assert
        Assert.Equal(expectedLenght, actualLenght);
        Assert.Equal(name, actualItem.Name);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(100)]
    public void GetAll_ShouldGetCorrectNumberOfItems(int expected)
    {
        // Arrange
        var service = new ShoppingListService();
        for(int i = 0; i < expected; i++)
            service.Add("Apples", 10, "Pink Lady");
        // Act
        var result = service.GetAll();
        // Assert
        Assert.Equal(expected, result.Count);
    }

    [Fact]
    public void GetById_ShouldReturnitemWithSameId()
    {
        //arrange
        var service = new ShoppingListService();
        var item = service.Add("Smör", 4, "extra saltat");

        //act
        var result = service.GetById(item.Id);

        //assert
        Assert.Equal(item.Id, result.Id);
    }

    [Fact]
    public void GetById_WithInvalid_shouldReturnNull()
    {
        //arrange
        var service = new ShoppingListService();
        service.Add("Smör", 4, "extra saltat");
        //act
        var result = service.GetById("invalid-id");
        //assert
        Assert.Null(result);
    }

    [Fact]
    public void Delete_ShouldReturnTrueIfDeleted()
    {
        // Arange
        var service = new ShoppingListService();
        var item = service.Add("Apples", 10, "Pink Lady");
        // Act
        var actual = service.Delete(item.Id); 
        // Assert
        Assert.True(actual);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(100)]
    public void Delete_ShouldDeleteItem(int numberOfData)
    {
        // Arange
        var service = new ShoppingListService();
        var item = service.Add("Apples", 10, "Pink Lady");
        for (int i = 0; i < numberOfData; i++)
            service.Add("Apples", 10, "Pink Lady");
        var actualLenghtBeforeDelete = service.GetAll().Count;
        // Act
        var Isdeleted = service.Delete(item.Id);
        var actualLenghtAfterDelete = service.GetAll().Count;
        var actual = service.GetById(item.Id);
        // Assert
        Assert.True(Isdeleted);
        Assert.Equal(null, actual);
        Assert.Equal(actualLenghtBeforeDelete -1, actualLenghtAfterDelete);
    }

    [Fact]
    public void Search_ShouldReturnMatchingItems()
    {
        // Arrange
        var service = new ShoppingListService();
        service.Add("Milk", 2, "Lactose-free");
        service.Add("Bread", 4, "Whole grain");
        service.Add("Juice", 5, "Orange juice");
        // Act
        var actual = service.Search("bread");
        var actual2 = service.Search("Milk");
        // Assert
        Assert.Single(actual);
        Assert.Equal("Bread", actual[0].Name);
        Assert.Equal("Milk", actual2[0].Name);
        Assert.Equal("Whole grain", actual[0].Notes);
        Assert.Equal("Lactose-free", actual2[0].Notes);

        }

}

