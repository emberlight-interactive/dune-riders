using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DuneRiders.GatheringSystem {
	public class AdditionRow : MonoBehaviour
	{
		[SerializeField] AdditionRow overflowRow;
		[SerializeField] float secondsBeforeStaleRenderClears = 2f;
		[SerializeField] TextMeshProUGUI amountText;
		[SerializeField] Image amountIcon;

		[SerializeField] Sprite fuelIcon;
		[SerializeField] Color fuelColor;

		[SerializeField] Sprite scrapMetalIcon;
		[SerializeField] Color scrapMetalColor;

		[SerializeField] Sprite preciousMetalIcon;
		[SerializeField] Color preciousMetalColor;

		[HideInInspector] public float currentlyRenderingAmount = 0f;
		[HideInInspector] public Gatherer.SupportedResources currentlyRenderingResource;
		[HideInInspector] public float lastDelayedRenderClearStarted;

		void Awake() {
			ClearRender();
		}

		public void RenderAddition(Gatherer.SupportedResources resource, float amount, float clearTimerOverride = 0) {
			if (
				IsRendered() && currentlyRenderingResource == resource
				|| !IsRendered()
			) {
				var sprite = GetResourceIcon(resource);
				var color = GetResourceColor(resource);

				currentlyRenderingResource = resource;
				DisplayRender(color, sprite, amount);
				StopAllCoroutines();
				StartCoroutine(DelayedClearRender(clearTimerOverride == 0 ? secondsBeforeStaleRenderClears : clearTimerOverride));
			} else {
				overflowRow?.RenderAddition(resource, amount);
			}
		}

		Sprite GetResourceIcon(Gatherer.SupportedResources resource) {
			switch (resource) {
				case Gatherer.SupportedResources.Fuel:
					return fuelIcon;
				case Gatherer.SupportedResources.ScrapMetal:
					return scrapMetalIcon;
				case Gatherer.SupportedResources.PreciousMetal:
					return preciousMetalIcon;
				default:
					return fuelIcon;
			}
		}

		Color GetResourceColor(Gatherer.SupportedResources resource) {
			switch (resource) {
				case Gatherer.SupportedResources.Fuel:
					return fuelColor;
				case Gatherer.SupportedResources.ScrapMetal:
					return scrapMetalColor;
				case Gatherer.SupportedResources.PreciousMetal:
					return preciousMetalColor;
				default:
					return fuelColor;
			}
		}

		void DisplayRender(Color color, Sprite icon, float amount) {
			currentlyRenderingAmount += amount;

			amountText.color = color;
			amountText.text = $"+{(int) currentlyRenderingAmount}";
			amountText.enabled = true;

			amountIcon.color = color;
			amountIcon.sprite = icon;
			amountIcon.enabled = true;
		}

		public void ClearRender() {
			currentlyRenderingAmount = 0f;
			amountText.enabled = false;
			amountIcon.enabled = false;

			ShiftOverflowRendersDown();
		}

		IEnumerator DelayedClearRender(float secondsUntilClear) {
			lastDelayedRenderClearStarted = Time.time;
			yield return new WaitForSeconds(secondsUntilClear);
			ClearRender();
		}

		public bool IsRendered() {
			return currentlyRenderingAmount > 0;
		}

		void ShiftOverflowRendersDown() {
			var isOverflowRendered = overflowRow?.IsRendered();
			if (isOverflowRendered != null && (bool) isOverflowRendered) {
				var lengthOfTimeClearTimerWasActive = Time.time - overflowRow.lastDelayedRenderClearStarted;
				if (lengthOfTimeClearTimerWasActive < secondsBeforeStaleRenderClears) {
					RenderAddition(overflowRow.currentlyRenderingResource, overflowRow.currentlyRenderingAmount, secondsBeforeStaleRenderClears - lengthOfTimeClearTimerWasActive);
					overflowRow.ClearRender();
				}
			}
		}
	}
}
