namespace Jetsparrow.MiniKeyboard;

public class ProductDescr
{
	public int VendorId { get; set; }
	public int ProductId { get; set; }
	public int ProtocolType { get; set; }
	public string SearchPathSegment { get; set; }
}

public static class ProductRegistry
{
    public static IReadOnlyDictionary<(int vendorId, int productId), ProductDescr> ProductsById { get; }
    public static IReadOnlyList<ProductDescr> Products => m_Products;

    static ProductRegistry()
    {
        ProductsById = Products.ToDictionary(p => (p.VendorId, p.ProductId));
    }

    const int ThisVendorId = 0x1189; // TODO RENAME
    static readonly List<ProductDescr> m_Products = new ()
    {
        new() {
            VendorId = ThisVendorId,
            ProductId = 0x8890,
            ProtocolType = 0,
            SearchPathSegment = "mi_01"
        },
        new() {
            VendorId = ThisVendorId,
            ProductId = 34864,
            ProtocolType = 1,
            SearchPathSegment = "mi_00"
        },
        new() {
            VendorId = ThisVendorId,
            ProductId = 34865,
            ProtocolType = 1,
            SearchPathSegment = "mi_00"
        },
        new() {
            VendorId = ThisVendorId,
            ProductId = 34866,
            ProtocolType = 1,
            SearchPathSegment = "mi_00"
        },
        new() {
            VendorId = ThisVendorId,
            ProductId = 34867,
            ProtocolType = 1,
            SearchPathSegment = "mi_00"
        },
        new() {
            VendorId = ThisVendorId,
            ProductId = 34832,
            ProtocolType = 1,
            SearchPathSegment = "mi_00"
        },
    };

}