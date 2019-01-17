using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using GameJolt.API;
using GameJolt.API.Objects;
using UnityEngine.SceneManagement;


namespace GameJolt.UI.Controllers {
	public class LeaderboardsWindow : BaseWindow {
		public RectTransform TabsContainer;
		public GameObject TableButton;

		public ScrollRect ScoresScrollRect;
		public GameObject ScoreItem;

		private Action<bool> callback;

		private int[] tableIDs;
		private int currentTab;
        public GameObject loadingPrefab;
        private GameObject load;

		public override void Show(Action<bool> callback) {
            Show(callback, null, null);
		}

		public void Show(Action<bool> callback, int? activeTable, int[] visibleTables) {
            //Animator.SetTrigger("ShowLoadingIndicator");
            load = Instantiate(loadingPrefab, Vector3.zero , Quaternion.identity, transform);
            RectTransform rectTransform = load.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = Vector2.zero;

			this.callback = callback;

			Scores.GetTables(tables => {
				// preprocess tables to match the visible tables provided by the user
				if(tables != null && visibleTables != null && visibleTables.Length > 0) {
					tables = tables.Where(x => visibleTables.Contains(x.ID)).ToArray();
				}
				if(tables != null && tables.Length > 0) {
					// Create the right number of children.
					Populate(TabsContainer, TableButton, tables.Length);
					int activeId = GetActiveTableId(tables, activeTable);

					// Update children's text. 
					tableIDs = new int[tables.Length];
					for(int i = 0; i < tables.Length; ++i) {
						TabsContainer.GetChild(i).GetComponent<TableButton>().Init(tables[i], i, this, tables[i].ID == activeId);

						// Keep IDs information and current tab for use when switching tabs.
						tableIDs[i] = tables[i].ID;
						if(tables[i].ID == activeId) {
							currentTab = i;
						}
					}

					SetScores(activeId);
				} else {
                    // TODO: Show error notification
                    //Animator.SetTrigger("HideLoadingIndicator");
                    Destroy(load);
					Dismiss(false);
				}
			});
		}

		private int GetActiveTableId(Table[] tables, int? activeTable) {
			if(activeTable.HasValue && tables.Any(x => x.ID == activeTable.Value)) // try to use the provided table id
				return activeTable.Value;
			// try to find the primary table
			var primary = tables.FirstOrDefault(x => x.Primary);
			if(primary != null) return primary.ID;
			// the first table is used as a fallback
			return tables[0].ID;
		}

		public override void Dismiss(bool success) {
            SceneManager.LoadScene("Menu");
        }

        public void ShowTab(int index) {
			// There is no need to set the new tab button active, it has been done internally when the button has been clicked.
		/*	TabsContainer.GetChild(currentTab).GetComponent<TableButton>().SetActive(false);
			currentTab = index;

			Animator.SetTrigger("Lock");
			Animator.SetTrigger("ShowLoadingIndicator");

			// Request new scores.
			SetScores(tableIDs[currentTab]);*/
		}

		private void SetScores(int tableId) {
			Scores.Get(scores => {
				if(scores != null) {
					ScoresScrollRect.verticalNormalizedPosition = 0;

					// Create the right number of children.
					Populate(ScoresScrollRect.content, ScoreItem, scores.Length);

					// Update children's text.
					for(int i = 0; i < scores.Length; ++i) {
						ScoresScrollRect.content.GetChild(i).GetComponent<ScoreItem>().Init(scores[i]);
					}

					Animator.SetTrigger("HideLoadingIndicator");
                    Destroy(load);
                    //Animator.SetTrigger("Unlock");
                } else {
					// TODO: Show error notification
					Animator.SetTrigger("HideLoadingIndicator");
                    Destroy(load);
					Dismiss(false);
				}
			}, tableId, 50);
		}
	}
}