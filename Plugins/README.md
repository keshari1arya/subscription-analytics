# Plugins Folder

This folder is used for connector plugin DLLs (e.g., Stripe, PayPal, etc.) for SubscriptionAnalytics.

## Usage
- Place all built connector plugin DLLs here (e.g., `SubscriptionAnalytics.Connectors.Stripe.dll`).
- The host applications (Api, Worker, Webhook) will scan this folder at runtime and load all valid connector plugins.
- Each plugin must implement the required interfaces from `SubscriptionAnalytics.Shared` (e.g., `IConnector`).

## Naming Conventions
- Plugin DLLs should be named as `SubscriptionAnalytics.Connectors.<Provider>.dll` (e.g., `SubscriptionAnalytics.Connectors.Stripe.dll`).

## Versioning
- Ensure plugin DLLs are built against the same version of `SubscriptionAnalytics.Shared` as the host applications.

## Source Control
- This folder is tracked in git for structure, but plugin DLLs themselves should not be committed.
- A `.gitkeep` file may be added to keep the folder in version control if empty. 