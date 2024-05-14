using Grpc.Core;

using Mapster;

using Microsoft.EntityFrameworkCore;

namespace Discount.Grpc.Services;

public class DiscountService(
    DiscountContext context,
    ILogger<DiscountService> logger)
    : DiscountProtoService.DiscountProtoServiceBase
{
    private readonly DiscountContext _context = context;
    private readonly ILogger<DiscountService> _logger = logger;

    public override async Task<CouponModel> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _context
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName)
            .ConfigureAwait(false);

        coupon ??= Coupon.NoDiscount();

        _logger.LogInformation("Discount is retrieved for ProductName : {productName}, Amount : {amount}", coupon.ProductName, coupon.Amount);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>()
            ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        _context.Coupons.Add(coupon);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Discount is successfully created. ProductName : {ProductName}", coupon.ProductName);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<CouponModel> UpdateDiscount(UpdateDiscountRequest request, ServerCallContext context)
    {
        var coupon = request.Coupon.Adapt<Coupon>()
            ?? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        _context.Coupons.Update(coupon);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Discount is successfully updated. ProductName : {ProductName}", coupon.ProductName);

        return coupon.Adapt<CouponModel>();
    }

    public override async Task<DeleteDiscountResponse> DeleteDiscount(DeleteDiscountRequest request, ServerCallContext context)
    {
        var coupon = await _context
            .Coupons
            .FirstOrDefaultAsync(x => x.ProductName == request.ProductName)
            .ConfigureAwait(false)
            ?? throw new RpcException(new Status(StatusCode.NotFound, $"Discount with ProductName={request.ProductName} is not found."));

        _context.Coupons.Remove(coupon);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Discount is successfully deleted. ProductName : {ProductName}", request.ProductName);

        return new DeleteDiscountResponse { Success = true };
    }
}
