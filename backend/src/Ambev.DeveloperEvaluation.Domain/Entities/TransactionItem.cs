using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Transaction.Validation;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

/// <summary>
/// Represents an item within a commercial transaction with business rules for discounts and quantity limits
/// </summary>
public class TransactionItem : BaseEntity
{
    /// <summary>
    /// Identification for the product
    /// </summary>
    public Guid ProductId { get; set; }

    /// <summary>
    /// Product being sold information
    /// </summary>
    public Product Product { get; set; } = null!;

    /// <summary>
    /// Quantity of the product being sold
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Item price at the time of transaction
    /// </summary>
    public decimal ItemPrice { get; set; }

    /// <summary>
    /// Discount percentage applied (0-100)
    /// </summary>
    public decimal DiscountPercentage { get; set; }

    /// <summary>
    /// Total amount for this item (quantity * item price * (1 - discount/100))
    /// </summary>
    public decimal ItemTotal { get; set; }

    /// <summary>
    /// Indicates if this item has been cancelled
    /// </summary>
    public bool IsCancelled { get; set; }

    /// <summary>
    /// Reference to the transaction this item belongs to
    /// </summary>
    public Guid TransactionId { get; set; }

    /// <summary>
    /// Initializes a new instance of the TransactionItem class
    /// </summary>
    public TransactionItem()
    {
    }

    /// <summary>
    /// Creates a new transaction item with automatic discount calculation
    /// </summary>
    /// <param name="productId">Product identifier</param>
    /// <param name="quantity">Quantity of items</param>
    /// <param name="itemPrice">Item price</param>
    /// <returns>New transaction item instance</returns>
    public static TransactionItem Create(Guid productId, int quantity, decimal itemPrice)
    {
        var item = new TransactionItem
        {
            ProductId = productId,
            Quantity = quantity,
            ItemPrice = itemPrice,
            IsCancelled = false
        };
        
        item.CalculateDiscount();
        item.CalculateItemTotal();
        
        return item;
    }

    /// <summary>
    /// Calculates discount based on business rules
    /// <remarks>
    /// 20% discount for 10-20 items
    /// 10% discount for 4+ items
    /// No discount for less than 4 items
    /// </remarks>
    /// </summary>
    public void CalculateDiscount()
    {
        DiscountPercentage = Quantity switch
        {
            >= 10 and <= 20 => 20m, 
            >= 4 => 10m,             
            _ => 0m                  
        };
    }

    /// <summary>
    /// Calculates total amount for this item
    /// </summary>
    public void CalculateItemTotal()
    {
        ItemTotal = Quantity * ItemPrice * (1 - DiscountPercentage / 100);
    }

    /// <summary>
    /// Updates the quantity and recalculates discount and total
    /// </summary>
    /// <param name="newQuantity">New quantity</param>
    public void UpdateQuantity(int newQuantity)
    {
        if (newQuantity > 20)
            throw new DomainException("Cannot sell more than 20 identical items");

        if (newQuantity <= 0)
            throw new DomainException("Quantity must be greater than zero");

        Quantity = newQuantity;
        CalculateDiscount();
        CalculateItemTotal();
    }

    /// <summary>
    /// Cancels this transaction item
    /// </summary>
    public void Cancel()
    {
        IsCancelled = true;
    }

    /// <summary>
    /// Validates the transaction item using business rules
    /// </summary>
    /// <returns>Validation result</returns>
    public ValidationResultDetail Validate()
    {
        var validator = new TransactionItemValidator();
        var result = validator.Validate(this);
        return new ValidationResultDetail
        {
            IsValid = result.IsValid,
            Errors = result.Errors.Select(o => (ValidationErrorDetail)o)
        };
    }
}