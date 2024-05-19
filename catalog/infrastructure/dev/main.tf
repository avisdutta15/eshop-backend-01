terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "3.104.0"
    }
  }
}

provider "azurerm" {
  features {}
}

data "azurerm_subscription" "current" {}


###################################################################  dev #################################################################################

# Create resource group
resource "azurerm_resource_group" "rg_dev" {
  name     = "rg-eshop-catalog-dev"
  location = "West Europe"
}

#Create app service plan
resource "azurerm_service_plan" "app_service_plan_dev" {
  name                = "ASP-rgeshopcatalogdev-b3ea"
  resource_group_name = azurerm_resource_group.rg_dev.name
  location            = azurerm_resource_group.rg_dev.location
  sku_name            = "F1"
  os_type             = "Windows"
}

# Create web app service
resource "azurerm_app_service" "webapp_dev" {
  name                = "eshop-catalog-dev"
  location            = azurerm_resource_group.rg_dev.location
  resource_group_name = azurerm_resource_group.rg_dev.name
  app_service_plan_id = azurerm_service_plan.app_service_plan_dev.id
}

# Create User Assigned Managed Identity
resource "azurerm_user_assigned_identity" "github_managed_identity_dev" {
  location            = azurerm_resource_group.rg_dev.location
  name                = "eshopdevelopmentci-cd-eshop-sp-dev"
  resource_group_name = azurerm_resource_group.rg_dev.name
}

# Assign Contributor role to the manged identity for the resource group
resource "azurerm_role_assignment" "uami_role_assignment_dev" {
  scope                = azurerm_resource_group.rg_dev.id
  role_definition_name = "Contributor"
  principal_id         = azurerm_user_assigned_identity.github_managed_identity_dev.principal_id
}

# Create federated credential
resource "azurerm_federated_identity_credential" "fed_cred_dev" {
  name                = "github_eshop-backend-01_dev"
  resource_group_name = azurerm_resource_group.rg_dev.name
  audience            = ["api://AzureADTokenExchange"]
  issuer              = "https://token.actions.githubusercontent.com"
  parent_id           = azurerm_user_assigned_identity.github_managed_identity_dev.id
  subject             = "repo:avisdutta15/eshop-backend-01:environment:dev"
}

# These outputs are used to populate the github environment secrets.

# dev User Managed Identity's Client Id. Copy this and paste it as a secret value for key AZURE_CLIENT_ID in dev environment.
output "user_assigned_managed_identity_client_id_dev" {
  value = azurerm_user_assigned_identity.github_managed_identity_dev.client_id
}

# Tenant Id. Copy this and paste it as a secret value for key AZURE_TENANT_ID in repository secret.
output "user_assigned_managed_identity_tenant_id_dev" {
  value = data.azurerm_subscription.current.tenant_id
}

# Subscription Id. Copy this and paste it as a secret value for key AZURE_SUBSCRIPTION_ID in repository secret.
output "user_assigned_managed_identity_subscription_id_dev" {
  value = data.azurerm_subscription.current.subscription_id
}

###################################################################  PPD #################################################################################

# Create resource group
resource "azurerm_resource_group" "rg_ppd" {
  name     = "rg-eshop-catalog-ppd"
  location = "West Europe"
}

#Create app service plan
resource "azurerm_service_plan" "app_service_plan_ppd" {
  name                = "ASP-rgeshopcatalogppd-b3ea"
  resource_group_name = azurerm_resource_group.rg_ppd.name
  location            = azurerm_resource_group.rg_ppd.location
  sku_name            = "F1"
  os_type             = "Windows"
}

# Create web app service
resource "azurerm_app_service" "webapp_ppd" {
  name                = "eshop-catalog-ppd"
  location            = azurerm_resource_group.rg_ppd.location
  resource_group_name = azurerm_resource_group.rg_ppd.name
  app_service_plan_id = azurerm_service_plan.app_service_plan_ppd.id
}

# Create User Assigned Managed Identity
resource "azurerm_user_assigned_identity" "github_managed_identity_ppd" {
  location            = azurerm_resource_group.rg_ppd.location
  name                = "eshoppreproductionci-cd-eshop-sp-ppd"
  resource_group_name = azurerm_resource_group.rg_ppd.name
}

# Assign Contributor role to the manged identity for the resource group
resource "azurerm_role_assignment" "uami_role_assignment_ppd" {
  scope                = azurerm_resource_group.rg_ppd.id
  role_definition_name = "Contributor"
  principal_id         = azurerm_user_assigned_identity.github_managed_identity_ppd.principal_id
}

# Create federated credential
resource "azurerm_federated_identity_credential" "fed_cred_ppd" {
  name                = "github_eshop-backend-01_ppd"
  resource_group_name = azurerm_resource_group.rg_ppd.name
  audience            = ["api://AzureADTokenExchange"]
  issuer              = "https://token.actions.githubusercontent.com"
  parent_id           = azurerm_user_assigned_identity.github_managed_identity_ppd.id
  subject             = "repo:avisdutta15/eshop-backend-01:environment:ppd"
}

# These outputs are used to populate the github environment secrets.

# ppd User Managed Identity's Client Id. Copy this and paste it as a secret value for key AZURE_CLIENT_ID in ppd environment.
output "user_assigned_managed_identity_client_id_ppd" {
  value = azurerm_user_assigned_identity.github_managed_identity_ppd.client_id
}

# Tenant Id. Copy this and paste it as a secret value for key AZURE_TENANT_ID in repository secret.
output "user_assigned_managed_identity_tenant_id_ppd" {
  value = data.azurerm_subscription.current.tenant_id
}

# Subscription Id. Copy this and paste it as a secret value for key AZURE_SUBSCRIPTION_ID in repository secret.
output "user_assigned_managed_identity_subscription_id_ppd" {
  value = data.azurerm_subscription.current.subscription_id
}


###################################################################  PROD #################################################################################

# Create resource group
resource "azurerm_resource_group" "rg_prod" {
  name     = "rg-eshop-catalog-prod"
  location = "West Europe"
}

#Create app service plan
resource "azurerm_service_plan" "app_service_plan_prod" {
  name                = "ASP-rgeshopcatalogprod-b3ea"
  resource_group_name = azurerm_resource_group.rg_prod.name
  location            = azurerm_resource_group.rg_prod.location
  sku_name            = "F1"
  os_type             = "Windows"
}

# Create web app service
resource "azurerm_app_service" "webapp_prod" {
  name                = "eshop-catalog-prod"
  location            = azurerm_resource_group.rg_prod.location
  resource_group_name = azurerm_resource_group.rg_prod.name
  app_service_plan_id = azurerm_service_plan.app_service_plan_prod.id
}

# Create User Assigned Managed Identity
resource "azurerm_user_assigned_identity" "github_managed_identity_prod" {
  location            = azurerm_resource_group.rg_prod.location
  name                = "eshopproductionci-cd-eshop-sp-prod"
  resource_group_name = azurerm_resource_group.rg_prod.name
}

# Assign Contributor role to the manged identity for the resource group
resource "azurerm_role_assignment" "uami_role_assignment_prod" {
  scope                = azurerm_resource_group.rg_prod.id
  role_definition_name = "Contributor"
  principal_id         = azurerm_user_assigned_identity.github_managed_identity_prod.principal_id
}

# Create federated credential
resource "azurerm_federated_identity_credential" "fed_cred_prod" {
  name                = "github_eshop-backend-01_prod"
  resource_group_name = azurerm_resource_group.rg_prod.name
  audience            = ["api://AzureADTokenExchange"]
  issuer              = "https://token.actions.githubusercontent.com"
  parent_id           = azurerm_user_assigned_identity.github_managed_identity_prod.id
  subject             = "repo:avisdutta15/eshop-backend-01:environment:prod"
}

# These outputs are used to populate the github environment secrets.

# Prod User Managed Identity's Client Id. Copy this and paste it as a secret value for key AZURE_CLIENT_ID in prod environment.
output "user_assigned_managed_identity_client_id_prod" {
  value = azurerm_user_assigned_identity.github_managed_identity_prod.client_id
}

# Tenant Id. Copy this and paste it as a secret value for key AZURE_TENANT_ID in repository secret.
output "user_assigned_managed_identity_tenant_id_prod" {
  value = data.azurerm_subscription.current.tenant_id
}

# Subscription Id. Copy this and paste it as a secret value for key AZURE_SUBSCRIPTION_ID in repository secret.
output "user_assigned_managed_identity_subscription_id_prod" {
  value = data.azurerm_subscription.current.subscription_id
}