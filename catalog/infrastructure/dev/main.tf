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

# Create resource group
resource "azurerm_resource_group" "rg" {
  name     = "rg-eshop-catalog-prod"
  location = "West Europe"
}

#Create app service plan
resource "azurerm_service_plan" "app_service_plan" {
  name                = "ASP-rgeshopcatalogdev-b3ea"
  resource_group_name = azurerm_resource_group.rg.name
  location            = azurerm_resource_group.rg.location
  sku_name            = "F1"
  os_type             = "Windows"
}

# Create web app service
resource "azurerm_app_service" "webapp" {
  name                = "eshop-catalog-prod"
  location            = azurerm_resource_group.rg.location
  resource_group_name = azurerm_resource_group.rg.name
  app_service_plan_id = azurerm_service_plan.app_service_plan.id
}

# Create User Assigned Managed Identity
resource "azurerm_user_assigned_identity" "github_managed_identity" {
  location            = azurerm_resource_group.rg.location
  name                = "eshopproductionci-cd-eshop-sp-prod"
  resource_group_name = azurerm_resource_group.rg.name
}

# Assign Contributor role to the manged identity for the resource group
resource "azurerm_role_assignment" "uami_role_assignment" {
  scope                = azurerm_resource_group.rg.id
  role_definition_name = "Contributor"
  principal_id         = azurerm_user_assigned_identity.github_managed_identity.principal_id
}

# Create federated credential
resource "azurerm_federated_identity_credential" "example" {
  name                = "github_eshop-backend-01_prod"
  resource_group_name = azurerm_resource_group.rg.name
  audience            = ["api://AzureADTokenExchange"]
  issuer              = "https://token.actions.githubusercontent.com"
  parent_id           = azurerm_user_assigned_identity.github_managed_identity.id
  subject             = "repo:avisdutta15/eshop-backend-01:environment:prod"
}