using ScrumAble.Areas.Identity.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ScrumAble.Models
{
    public class ViewModelTaskAggregate
    {
        [NotMapped]
        public ScrumAbleTask Task { get; set; }

        [NotMapped]
        public ICollection<ScrumAbleUser> Users { get; set; }

        [NotMapped]
        public ICollection<ScrumAbleSprint> Sprints { get; set; }

        [NotMapped]
        public ICollection<ScrumAbleStory> Stories { get; set; }

        public void Add(ScrumAbleTask scrumAbleTask)
        {
            Task = scrumAbleTask;
        }

        public void Add(ICollection<ScrumAbleUser> scrumAbleUser)
        {
            Users = scrumAbleUser;
        }

        public void Add(ICollection<ScrumAbleSprint> scrumAbleSprint)
        {
            Sprints = scrumAbleSprint;
        }

        public void Add(ICollection<ScrumAbleStory> scrumAbleStory)
        {
            Stories = scrumAbleStory;
        }
    }


}
