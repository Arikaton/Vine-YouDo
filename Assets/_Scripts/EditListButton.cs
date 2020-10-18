using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class EditListButton : MonoBehaviour
    {
        private static EditListButton _active;
        
        [SerializeField] private Image image;
        [SerializeField] private Text nameText;
        [SerializeField] private Image favoriteImage;
        [SerializeField] private GameObject menu;

        private string _objectId;
        public bool IsFavorite { get; private set; }
        public string Name { get; private set; }

        public void Init(Color color, string name, bool isFavorite, string objectId)
        {
            Name = name;
            image.color = color;
            nameText.text = name;
            _objectId = objectId;
            IsFavorite = isFavorite;
            UpdateFavoriteIcon();
        }

        public void ShowMenu()
        {
            if (_active != null)
            {
                if (_active != this)
                {
                    _active.HideMenu();
                    _active = this;
                    menu.SetActive(true);
                }
                else
                {
                    menu.SetActive(!menu.activeSelf);
                }
            }
            else
            {
                _active = this;
                menu.SetActive(true); 
            }
        }

        public void HideMenu()
        {
            menu.SetActive(false);
        }

        public void Delete()
        {
            Debug.Log(_objectId);
            GetFromListHelper.Current.DeleteObject(_objectId);
            Destroy(gameObject);
        }

        public void UpdateFavorite()
        {
            HideMenu();
            IsFavorite = !IsFavorite;
            GetFromListHelper.Current.UpdateFavorite(IsFavorite, _objectId);
            UpdateFavoriteIcon();
            GetFromListHelper.Current.FilterListByFavorite();
        }

        private void UpdateFavoriteIcon()
        {
            if (IsFavorite)
                favoriteImage.color = Color.white;
            else
                favoriteImage.color = new Color(1, 1, 1, 0);
        }
    }
}