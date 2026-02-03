using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Enums;
using Ambev.DeveloperEvaluation.Domain.Transaction.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents a commercial transaction with business rules and validations
/// </summary>
public class CommercialTransaction : BaseEntity
{
    /// <summary>
    /// Unique transaction code for identification
    /// </summary>
    public string TransactionCode { get; set; } = string.Empty;

    /// <summary>
    /// Date when the transaction was made
    /// </summary>
    public DateTime TransactionDate { get; set; }

    /// <summary>
    /// Business partner identification
    /// </summary>
    public Guid BusinessPartnerId { get; set; }

    /// <summary>
    /// Business Partner reference
    /// </summary>
    public BusinessPartner BusinessPartner { get; set; } = null!;

    /// <summary>
    /// Operational unit identification
    /// </summary>
    public Guid OperationalUnitId { get; set; }

    /// <summary>
    /// Operational unit reference 
    /// </summary>
    public OperationalUnit OperationalUnit { get; set; } = null!;

    /// <summary>
    /// Grand total amount of the transaction
    /// </summary>
    public decimal Amount { get; set; }

    /// <summary>
    /// Current status of the transaction
    /// </summary>
    public TransactionStatus Status { get; set; }

    /// <summary>
    /// Collection of items in this transaction
    /// </summary>
    public List<TransactionItem> Items { get; set; } = new();

    /// <summary>
    /// Date when the transaction was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Date when the transaction was last updated
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Initializes a new instance of the CommercialTransaction class
    /// </summary>
    public CommercialTransaction()
    {
        CreatedAt = DateTime.UtcNow;
        Status = TransactionStatus.Active;
        TransactionDate = DateTime.UtcNow;
    }

    /// <summary>
    /// Adds an item to the transaction
    /// </summary>
    /// <param name="productId">Product identifier</param>
    /// <param name="quantity">Quantity to add</param>
    /// <param name="itemPrice">Item price</param>
    public void AddItem(Guid productId, int quantity, decimal itemPrice)
    {
        if (Status is TransactionStatus.Cancelled)
            throw new DomainException("Cannot add items to a cancelled transaction");

        if (quantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items");

        var existingItem = Items.FirstOrDefault(i => i.ProductId == productId && !i.IsCancelled);
        
        if (existingItem is not null)
        {
            var newQuantity = existingItem.Quantity + quantity;
            if (newQuantity > 20)
                throw new DomainException("Cannot sell more than 20 identical items");
            
            existingItem.UpdateQuantity(newQuantity);
        }
        else
        {
            var newItem = TransactionItem.Create(productId, quantity, itemPrice);
            newItem.TransactionId = Id;
            Items.Add(newItem);
        }

        RecalculateGrandTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Updates the quantity of an existing item
    /// </summary>
    /// <param name="itemId">Item identifier</param>
    /// <param name="newQuantity">New quantity</param>
    public void UpdateItemQuantity(Guid itemId, int newQuantity)
    {
        if (Status is TransactionStatus.Cancelled)
            throw new DomainException("Cannot update items in a cancelled transaction");

        var item = Items.FirstOrDefault(i => i.Id == itemId && !i.IsCancelled);
        if (item is null)
            throw new DomainException("Item not found or already cancelled");

        item.UpdateQuantity(newQuantity);
        RecalculateGrandTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels a specific item in the transaction
    /// </summary>
    /// <param name="itemId">Item identifier</param>
    public void CancelItem(Guid itemId)
    {
        if (Status is TransactionStatus.Cancelled)
            throw new DomainException("Cannot cancel items in a cancelled transaction");

        var item = Items.FirstOrDefault(i => i.Id == itemId && !i.IsCancelled);
        if (item is null)
            throw new DomainException("Item not found or already cancelled");

        item.Cancel();
        RecalculateGrandTotal();
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Cancels the entire transaction
    /// </summary>
    public void Cancel()
    {
        Status = TransactionStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }

    /// <summary>
    /// Recalculates the grand total based on active items
    /// </summary>
    private void RecalculateGrandTotal()
    {
        Amount = Items.Where(i => !i.IsCancelled).Sum(i => i.ItemTotal);
    }

    /// <summary>
    /// Validates the transaction using business rules
    /// </summary>
    /// <returns>Validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new CommercialTransactionValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}