/* eslint-disable react/prop-types */
import { CreditCard } from "lucide-react";

const OrderSummary = ({ cartItems, onCheckout }) => {
  const calculateSubtotal = () => {
    return cartItems.reduce(
      (total, item) => total + item.product.price * item.quantity,
      0
    );
  };

  const subtotal = calculateSubtotal();
  const tax = subtotal * 0.1;
  const total = subtotal + tax;

  return (
    <div className="card bg-base-100 shadow-xl sticky top-8">
      <div className="card-body">
        <h2 className="card-title text-2xl mb-4">Order Summary</h2>
        <div className="space-y-3">
          <div className="flex justify-between text-base-content/70">
            <span>Subtotal</span>
            <span>${subtotal.toFixed(2)}</span>
          </div>
          <div className="flex justify-between text-base-content/70">
            <span>Shipping</span>
            <span className="text-success">Free</span>
          </div>
          <div className="flex justify-between text-base-content/70">
            <span>Tax</span>
            <span>${tax.toFixed(2)}</span>
          </div>
          <div className="divider my-2"></div>
          <div className="flex justify-between text-lg font-bold">
            <span>Total</span>
            <span>${total.toFixed(2)}</span>
          </div>
        </div>

        <div className="card-actions mt-6">
          <button
            className="btn btn-primary btn-block btn-lg gap-2"
            onClick={onCheckout}>
            <CreditCard size={20} />
            Checkout
          </button>
          <button className="btn btn-ghost btn-block">Continue Shopping</button>
        </div>

        <div className="mt-4">
          <div className="flex items-center gap-2 text-base-content/50 text-sm justify-center">
            <span>Secure payment powered by</span>
            <span className="font-semibold">Stripe</span>
          </div>
        </div>
      </div>
    </div>
  );
};

export default OrderSummary;
